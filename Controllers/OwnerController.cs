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

    public class OwnerController :ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository country;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper,ICountryRepository country)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            this.country = country;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public IActionResult GetOwners()
        {
            var pokemon = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }



        [HttpGet("{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int OwnerId)
        {
            if (!_ownerRepository.OwnerExists(OwnerId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(OwnerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpGet("{OwnerId}/Pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int OwnerId)
        {
            if (!_ownerRepository.OwnerExists(OwnerId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<List<PokemonDTO>>(_ownerRepository.GetPokemonByOwner(OwnerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpGet("Pokemon/{PokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerByPokemon(int PokeId)
        {
            if (!_ownerRepository.OwnerExists(PokeId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwnerByPokemon(PokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int id,[FromBody] OwnerDTO ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

       var owner = _ownerRepository.GetOwners().
                Where(c => (c.FirstName+c.LastName).Trim().ToUpper() == (ownerCreate.FirstName + ownerCreate.LastName).Trim().ToUpper()).FirstOrDefault();
            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var OwnerMap = _mapper.Map<Owner>(ownerCreate);
            OwnerMap.Country = country.GetCountry(id);
            if (!_ownerRepository.CreateOwner(OwnerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }

        [HttpPut("{OwnerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int OwnerId, [FromBody] OwnerDTO updateOwner)
        {
            if (updateOwner == null)
            {

                return BadRequest(ModelState);
            }

            if (OwnerId != updateOwner.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepository.OwnerExists(OwnerId))
            {
                return NotFound();
            }
            var owner = _mapper.Map<Owner>(updateOwner);
            if (!_ownerRepository.UpdateOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{OwnerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int OwnerId)
        {
            if (OwnerId  == null)
            {
                return BadRequest(ModelState);
            }


            if (!_ownerRepository.OwnerExists(OwnerId))
            {
                return NotFound();
            }


            if (!_ownerRepository.DeleOwner(OwnerId))
            {
                ModelState.AddModelError("", "Something went wrong with saving");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


    }
}
