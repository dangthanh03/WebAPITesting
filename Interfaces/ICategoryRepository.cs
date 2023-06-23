using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        public Category GetCategory(int id);
        public ICollection<Category> GetCategories();
        public ICollection<Pokemon> GetPokemonByCategory(int id);
        public bool CategoryExists(int id);

        public bool CreateCategory(Category cate);
        public bool UpdateCategory(Category cate);
        public bool DeleteCategory(int id);
    }
}
