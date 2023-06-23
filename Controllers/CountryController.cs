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
    public class CountryController:ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
          _countryRepository = countryRepository;   
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]

        public IActionResult GetCounrtries()
        {
            var pokemon = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]

        public IActionResult GetCounrtry(int id)
        {
            if (!_countryRepository.CountryExists(id)) {
                return NotFound();

                 }
            var pokemon = _mapper.Map<CountryDTO>(_countryRepository.GetCountry(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }


        [HttpGet("{id}/Owners")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public IActionResult GetOwner(int id)
        {
            if (!_countryRepository.CountryExists(id))
            {
                return NotFound();

            }
            var pokemon = _mapper.Map<List<OwnerDTO>>(_countryRepository.GetOwnersByCountry(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpGet("Owner/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public IActionResult GetCountryByOwner(int id)
        {
           
            var pokemon = _mapper.Map<CountryDTO>(_countryRepository.GetCountryByOwner(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries().
                Where(c => c.Name.Trim().ToUpper() ==countryCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CountryMap = _mapper.Map<Country>(countryCreate);
            if (!_countryRepository.CreateCountry(CountryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO updateCountry)
        {
            if (updateCountry == null)
            {

                return BadRequest(ModelState);
            }

            if (countryId != updateCountry.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var country= _mapper.Map<Country>(updateCountry);
            if (!_countryRepository.UpdateCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong updating country");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{CountryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry( int CountryId)
        {
            if (CountryId == null)
            {
                return BadRequest(ModelState);
            }


            if (!_countryRepository.CountryExists(CountryId))
            {
                return NotFound();
            }


            if (!_countryRepository.DeleteCountry(CountryId))
            {
                ModelState.AddModelError("", "Something went wrong with saving");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
