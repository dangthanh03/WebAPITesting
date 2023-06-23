using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController: ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository pokemonRepository;
        private readonly IReviewerRepository reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository,IMapper mapper,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            this.pokemonRepository = pokemonRepository;
            this.reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()
        {
            var pokemon = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }
        [HttpGet("{ReviewId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int ReviewId)
        {
            if (!_reviewRepository.ReviewExists(ReviewId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(ReviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }
       
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromQuery] int Reviewer, [FromBody] ReviewDTO ReviewCreate)
        {
            if (ReviewCreate == null)
            {
                return BadRequest(ModelState);
            }

            var Review = _reviewRepository.GetReviews().
                Where(r => r.Id == ReviewCreate.Id).FirstOrDefault();
            if (Review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ReviewMap = _mapper.Map<Review>(ReviewCreate);
           ReviewMap.Reviewer = reviewerRepository.GetReviewer(Reviewer);
            if (!_reviewRepository.CreateReview(ReviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }
        [HttpPut("{ReviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int ReviewId, [FromBody] ReviewDTO ReviewUpdate)
        {
            if (ReviewUpdate == null)
            {

                return BadRequest(ModelState);
            }

            if (ReviewId != ReviewUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewRepository.ReviewExists(ReviewId))
            {
                return NotFound();
            }
            var review = _mapper.Map<Review>(ReviewUpdate);
            if (!_reviewRepository.UpdateReview(review))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{ReviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePoke(int ReviewId)
        {
            if (ReviewId == null)
            {
                return BadRequest(ModelState);
            }


            if (!_reviewRepository.ReviewExists(ReviewId))
            {
                return NotFound();
            }


            if (!_reviewRepository.DeleteReview(ReviewId))
            {
                ModelState.AddModelError("", "Something went wrong with saving");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
