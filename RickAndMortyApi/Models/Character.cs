using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace RickAndMortyApi.Models;

public class Character
{
    public int Id { get; set; } //	The id of the character.
    public string Name { get; set; } //	The name of the character.

    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; } //	The status of the character ('Alive', 'Dead' or 'unknown').

    public string Species { get; set; } //	The species of the character.
    public string Type { get; set; } //	The type or subspecies of the character.

    [JsonConverter(typeof(StringEnumConverter))]
    public Gender Gender { get; set; } //	The gender of the character ('Female', 'Male', 'Genderless' or 'unknown').

    public Origin Origin { get; set; } //	Name and link to the character's origin location.
    public Location Location { get; set; } //	Name and link to the character's last known location endpoint.
    public string Image { get; set; } // (url)	Link to the character's image. All images are 300x300px and most are medium shots or portraits since they are intended to be used as avatars.
    public List<string> Episode { get; set; } // (urls)	List of episodes in which this character appeared.
    public string Url { get; set; } // (url)	Link to the character's own URL endpoint.
    public string Created { get; set; } //	Time at which the character was created in the database.
}