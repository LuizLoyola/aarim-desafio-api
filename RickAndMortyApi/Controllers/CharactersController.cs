using Microsoft.AspNetCore.Mvc;
using RickAndMortyApi.Models;
using RickAndMortyApi.Services.Interfaces;

namespace RickAndMortyApi.Controllers;

[ApiController]
[Route("/characters")]
public class CharactersController : ControllerBase
{
    private ICharactersService CharactersService { get; }

    public CharactersController(ICharactersService charactersService)
    {
        CharactersService = charactersService;
    }

    [HttpGet("/list")]
    public async Task<IActionResult> ListAsync()
    {
        return Ok(await CharactersService.GetCharactersAsync());
    }

    [HttpGet("/filtered-list")]
    public async Task<IActionResult> FilteredListAsync()
    {
        var characters = await CharactersService.GetCharactersAsync();

        return Ok(characters.Where(c =>
            c.Status == Status.Unknown &&
            // don't know if is Alien or alien, use insensitive comparison
            string.Equals(c.Species, "Alien", StringComparison.CurrentCultureIgnoreCase) &&
            c.Episode.Count > 1
        ));
    }
}