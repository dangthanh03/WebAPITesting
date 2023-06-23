using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        public ICollection<Reviewer> GetReviewers();
        public Reviewer GetReviewer(int id);

        ICollection<Review> GetReviewsByReviewer(int ReviewerId);
        public bool ReviewerExists(int id);
        public bool CreateReviewer(Reviewer re);
        public bool UpdateReviewer(Reviewer re);
        public bool DeleteReviewer(int id); 
    }
}
