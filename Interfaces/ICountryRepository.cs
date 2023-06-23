using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        public Country GetCountry(int id);
        public ICollection<Country> GetCountries();
        public Country GetCountryByOwner(int OwnerId);
        public ICollection<Owner> GetOwnersByCountry(int CountryId);
        public bool UpdateCountry(Country Country);
        public bool CountryExists(int id);
        public bool CreateCountry(Country country);
        public bool DeleteCountry(int id);
    }
}
