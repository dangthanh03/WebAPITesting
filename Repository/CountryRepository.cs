using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {

        public CountryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public DataContext _dataContext { get; }

        public bool CountryExists(int id)
        {
           return _dataContext.Countries.Any(country => country.Id == id);  
        }

        public bool CreateCountry(Country country)
        {
            _dataContext.Add(country);
            return Save();
        }
        public bool Save()
        {
            var Country = _dataContext.SaveChanges();
            return Country > 0 ? true : false;
        }
        public ICollection<Country> GetCountries()
        {
            return _dataContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _dataContext.Countries.Where(c=>c.Id==id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _dataContext.Owners.Where(o => o.Id == OwnerId).Select(o => o.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountry(int CountryId)
        {
            return _dataContext.Owners.Where(o => o.Country.Id == CountryId).ToList();
        }

        public bool UpdateCountry(Country country)
        {
            _dataContext.Update(country);
            return Save();
        }

        public bool DeleteCountry(int id)
        {
            Country Country = _dataContext.Countries.Where(c=>c.Id==id).FirstOrDefault();
            _dataContext.Remove(Country);
            return Save();
        }
    }
}
