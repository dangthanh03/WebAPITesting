using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        public ICollection<Review> GetReviews();
        public Review GetReview(int id);

        public bool CreateReview(Review review);
        bool ReviewExists(int id);
        public bool UpdateReview(Review review);
        public bool DeleteReview(int id);
    }
}
