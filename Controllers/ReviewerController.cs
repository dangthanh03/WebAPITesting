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

    public class ReviewerController:ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository , IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]

        public IActionResult GetReviewers()
        {
            var pokemon = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }
        [HttpGet("{ReviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]

        public IActionResult GetReviewer(int ReviewerId)
        {
            var pokemon = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(ReviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpGet("{ReviewerId}/Reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviewsByReviewer(int ReviewerId)
        {
            var pokemon = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsByReviewer(ReviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer( [FromBody] ReviewerDTO ReviewerCreate)
        {
            if (ReviewerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var Reviewer = _reviewerRepository.GetReviewers().
                Where(p => p.Id == ReviewerCreate.Id).FirstOrDefault();
            if (Reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer  already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var PokemonMap = _mapper.Map<Reviewer>(ReviewerCreate);
            if (!_reviewerRepository.CreateReviewer( PokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }
        [HttpPut("{ReviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int ReviewerId, [FromBody] ReviewerDTO ReviewerUpdate)
        {
            if (ReviewerUpdate == null)
            {

                return BadRequest(ModelState);
            }

            if (ReviewerId != ReviewerUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewerRepository.ReviewerExists(ReviewerId))
            {
                return NotFound();
            }
            var Reviewer = _mapper.Map<Reviewer>(ReviewerUpdate);
            if (!_reviewerRepository.UpdateReviewer(Reviewer))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{ReviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePoke(int ReviewerId)
        {
            if (ReviewerId  == null)
            {
                return BadRequest(ModelState);
            }


            if (!_reviewerRepository.ReviewerExists(ReviewerId))
            {
                return NotFound();
            }


            if (!_reviewerRepository.DeleteReviewer(ReviewerId))
            {
                ModelState.AddModelError("", "Something went wrong with saving");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
