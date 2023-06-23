
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository( DataContext data)
        {
           _dataContext = data;
        }

        public bool CreateReview(Review review)
        {
            _dataContext.Add(review); ;
            return Save();
        }

        public bool Save()
        {
            var change = _dataContext.SaveChanges();
            return change > 0 ? true : false;
        }

        public Review GetReview(int id)
        {
            return _dataContext.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

      
        public ICollection<Review> GetReviews()
        {
            return _dataContext.Reviews.ToList();
        }

        public bool ReviewExists(int id)
        {
            return _dataContext.Reviews.Any(r=>r.Id==id);
        }

        public bool UpdateReview(Review review)
        {
            _dataContext.Update(review);
            return Save();
        }

        public bool DeleteReview(int id)
        {
          Review pokemon = _dataContext.Reviews.Where(c => c.Id == id).FirstOrDefault();
            _dataContext.Remove(pokemon);
            return Save();
        }
    }
}
