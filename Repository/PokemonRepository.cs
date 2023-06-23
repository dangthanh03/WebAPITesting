using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
      public PokemonRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Pokemon> GetPokemon()
        {
            return _context.Pokemon.OrderBy(p=> p.Id).ToList();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Id == pokeId);

            if (review.Count() <= 0)
            {
                return 0;
            }
         
        return  ((decimal)review.Sum(r=>r.Rating)/review.Count());
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool CreatePokemon(int OwnerId, int CateId, Pokemon pokemon)
        {
            var Owner = _context.Owners.Where(o => o.Id == OwnerId).FirstOrDefault();
            var Cate  = _context.Categories.Where(c=>c.Id==CateId).FirstOrDefault();
            var PokeOwner = new PokemonOwner
            {
                OwnerId = OwnerId,
                Owner = Owner,
                Pokemon = pokemon,
                PokemonId = pokemon.Id,

            };
            var CatePoke = new Models.PokemonCategory
            {
                   Pokemon= pokemon,
                   PokemonId= pokemon.Id,
                   Category = Cate,
                   CategoryId= CateId
            };

            _context.Add(PokeOwner);
            _context.Add(CatePoke);
            _context.Add(pokemon);
            return Save();
        }
        public bool Save()
        {
            var change = _context.SaveChanges();
            return change > 0 ? true : false;
        }

        public bool UpdatePokemon(Models.PokemonCategory PokeCate, PokemonOwner PokeOwn, Pokemon pokemon)
        {
            _context.Update(PokeCate);
            _context.Update(PokeOwn);
            _context.Update(pokemon);
            return Save();

        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(int pokeId)
        {
            Pokemon pokemon = _context.Pokemon.Where(c => c.Id == pokeId).FirstOrDefault();
            _context.Remove(pokemon);
            return Save();
        }
    }
}
