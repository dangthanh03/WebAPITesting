using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        public ICollection<Owner> GetOwners();
        public Owner GetOwner(int id);

        public ICollection<Pokemon> GetPokemonByOwner(int OwnerId);
        public ICollection<Owner> GetOwnerByPokemon(int PokeId);
        public bool OwnerExists(int id);
        public bool CreateOwner(Owner owner);
        public bool UpdateOwner(Owner owner);
        public bool DeleOwner(int id); 
    }
}
