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
            // don't know the casing, use insensitive comparison
            string.Equals(c.Status, "unknown", StringComparison.CurrentCultureIgnoreCase) &&
            string.Equals(c.Species, "alien", StringComparison.CurrentCultureIgnoreCase) &&
            c.Episode.Count > 1
        ));
    }
}