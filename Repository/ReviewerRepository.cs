using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateReviewer(Reviewer re)
        {
            _dataContext.Add(re);
            return Save();
        }
        public bool Save()
        {
            var change = _dataContext.SaveChanges();
            return change > 0 ? true : false;
        }

        public Reviewer GetReviewer(int id)
        {
            return _dataContext.Reviewers.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
         return _dataContext.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int ReviewerId)
        {
            return _dataContext.Reviews.Where(r => r.Reviewer.Id == ReviewerId).ToList();
        }

        public bool ReviewerExists(int id)
        {
            return _dataContext.Reviewers.Any(r=>r.Id==id);
        }

        public bool UpdateReviewer(Reviewer re)
        {
            _dataContext.Update(re);
            return Save();
        }

        public bool DeleteReviewer(int id)
        {
            Reviewer reviewer = _dataContext.Reviewers.Where(c => c.Id == id).FirstOrDefault();
            _dataContext.Remove(reviewer);
            return Save();
        }
    }
}
