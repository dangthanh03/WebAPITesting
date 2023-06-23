using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category cate)
        {
            _context.Add(cate);
           
            return Save();
        }

        public bool Save()
        {
            var change = _context.SaveChanges();
            return change > 0 ? true : false;

        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int id)
        {
            return _context.PokemonCategories.Where(p => p.CategoryId == id).Select(p => p.Pokemon).ToList();
        }

        public bool UpdateCategory(Category cate)
        {
            _context.Update(cate);
            return Save();
        }

        public bool DeleteCategory(int cate)
        {
            Category category= _context.Categories.Where(c => c.Id == cate).FirstOrDefault();
            _context.Remove(category);
            return Save();
        }
    }
}
