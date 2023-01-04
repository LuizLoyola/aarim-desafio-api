using Newtonsoft.Json.Linq;
using RickAndMortyApi.Models;
using RickAndMortyApi.Services.Interfaces;

namespace RickAndMortyApi.Services;

public class CharactersService : ICharactersService
{
    private HttpClient HttpClient { get; }

    private const string BaseUrl = "https://rickandmortyapi.com/api/character";

    public CharactersService(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public async Task<List<Character>> GetCharactersAsync()
    {
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

        return characters;
    }
}