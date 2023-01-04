using RickAndMortyApi.Models;

namespace RickAndMortyApi.Services.Interfaces;

public interface ICharactersService
{
    public Task<List<Character>> GetCharactersAsync();
}