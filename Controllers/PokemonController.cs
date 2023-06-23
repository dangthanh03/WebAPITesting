
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

    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepository pokemonRepository,IMapper mapper)
        {
           _pokemonRepository = pokemonRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemon()
        {
            var pokemon =mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemon());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
         public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon =mapper.Map<PokemonDTO>( _pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }
        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry( [FromQuery] int Ownerid, [FromQuery] int cateid ,  [FromBody] PokemonDTO PokemonCreate)
        {
            if (PokemonCreate == null)
            {
                return BadRequest(ModelState);
            }

            var Pokemon = _pokemonRepository.GetPokemon().
                Where(p =>p.Id == PokemonCreate.Id).FirstOrDefault();
            if (Pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var PokemonMap = mapper.Map<Pokemon>(PokemonCreate);
            if (!_pokemonRepository.CreatePokemon(Ownerid,cateid,PokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }
        [HttpPut("{PokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon (int PokeId, [FromBody] PokemonDTO PokemonUpdate)
        {
            if (PokemonUpdate== null)
            {

                return BadRequest(ModelState);
            }

            if (PokeId != PokemonUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_pokemonRepository.PokemonExists(PokeId))
            {
                return NotFound();
            }
            var pokemon = mapper.Map<Pokemon>(PokemonUpdate);
            if (!_pokemonRepository.UpdatePokemon(pokemon))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{PokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePoke(int PokemonId)
        {
            if (PokemonId == null)
            {
                return BadRequest(ModelState);
            }


            if (!_pokemonRepository.PokemonExists(PokemonId))
            {
                return NotFound();
            }


            if (!_pokemonRepository.DeletePokemon(PokemonId))
            {
                ModelState.AddModelError("", "Something went wrong with saving");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
