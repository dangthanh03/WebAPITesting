using Azure.Identity;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _datacontext;

        public OwnerRepository(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public bool CreateOwner(Owner owner)
        {
            _datacontext.Add(owner);
             return Save();
        }

        public bool Save()
        {
            var change = _datacontext.SaveChanges();
            return change > 0 ? true : false;
        }
        public Owner GetOwner(int id)
        {
            return _datacontext.Owners.Where(o=>o.Id==id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerByPokemon(int PokeId)
        {
            return _datacontext.PokemonOwners.Where(p=>p.PokemonId==PokeId).Select(p=>p.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _datacontext.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int OwnerId)
        {
            return _datacontext.PokemonOwners.Where(p => p.OwnerId == OwnerId).Select(p=>p.Pokemon).ToList();
        }
       
        public bool OwnerExists(int id)
        {
            return _datacontext.Owners.Any(o => o.Id == id);
        }

        public bool UpdateOwner(Owner owner)
        {
            _datacontext.Update(owner);
            return Save();
        }

        public bool DeleOwner(int id)
        {
            Owner owner= _datacontext.Owners.Where(c => c.Id == id).FirstOrDefault();
            _datacontext.Remove(owner);
            return Save();

        }
    }
}
