using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemon();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int pokeId);
        public bool CreatePokemon(int OwnerId,int CateId,Pokemon pokemon);
        public bool UpdatePokemon(Pokemon pokemon);
    public bool DeletePokemon(int pokeId);
    }
}
