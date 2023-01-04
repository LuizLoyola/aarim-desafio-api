using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMortyApi.Models;
using RickAndMortyApi.Services.Interfaces;

namespace RickAndMortyApi.Services;

public class CharactersService : ICharactersService
{
    private HttpClient HttpClient { get; }

    private const string BaseUrl = "https://rickandmortyapi.com/api/character";
    private const string CacheFile = "characters.json";

    public class CharacterCacheData
    {
        public CharacterCacheData()
        {
            Characters = new List<Character>();
            ExpiresAt = DateTime.MinValue;
        }

        public CharacterCacheData(List<Character> characters, int ttlMinutes = 5)
        {
            Characters = characters;
            ExpiresAt = DateTime.UtcNow.AddMinutes(ttlMinutes);
        }

        public List<Character> Characters { get; set; }
        public DateTime ExpiresAt { get; set; }

        [JsonIgnore] public bool IsValid => DateTime.UtcNow < ExpiresAt;
    }

    private CharacterCacheData? _cacheData;

    private CharacterCacheData CacheData
    {
        get => _cacheData ??= (File.Exists(CacheFile) ? JsonConvert.DeserializeObject<CharacterCacheData>(File.ReadAllText(CacheFile)) : null) ?? new CharacterCacheData();
        set
        {
            _cacheData = value;
            File.WriteAllText(CacheFile, JsonConvert.SerializeObject(_cacheData));
        }
    }

    public CharactersService(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public async Task<List<Character>> GetCharactersAsync()
    {
        if (CacheData.IsValid)
            return CacheData.Characters;

        var characters = new List<Character>();
        var nextPageUrl = BaseUrl;
        do
        {
            var response = await HttpClient.GetAsync(nextPageUrl);
            var content = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(content);
            var resultsArray = result["results"];

            if (resultsArray == null) throw new Exception("Error parsing response");

            // add characters to list
            characters.AddRange(resultsArray.ToObject<List<Character>>()!);

            var info = result["info"];
            if (info == null) throw new Exception("Error parsing response");

            // get next page url
            nextPageUrl = info["next"]?.ToString();
        } while (!string.IsNullOrEmpty(nextPageUrl));

        CacheData = new CharacterCacheData(characters);

        return characters;
    }
}