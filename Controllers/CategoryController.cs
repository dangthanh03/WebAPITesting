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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IMapper _mapper { get; }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]

        public IActionResult GetCategories()
        {
            var pokemon = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemon);
        }

        [HttpGet("{CateId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int CateId)
        {
            if (!_categoryRepository.CategoryExists(CateId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(CateId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);


            }
            return Ok(pokemon);
        }
        [HttpGet("{CateIdd}/Pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCate(int CateIdd)
        {
           
            var pokemon = _mapper.Map<List<PokemonDTO>>(_categoryRepository.GetPokemonByCategory(CateIdd));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);


            }
            return Ok(pokemon);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var category = _categoryRepository.GetCategories().
                Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("","Category already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CategoryMap = _mapper.Map<Category>(categoryCreate);
            if (!_categoryRepository.CreateCategory(CategoryMap))
            {
                ModelState.AddModelError("","Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully create");
        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDTO updateCategory)
        {
            if (updateCategory == null)
            {

                return BadRequest(ModelState);
            }

            if (categoryId != updateCategory.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }
            var category = _mapper.Map<Category>(updateCategory);
            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("","Something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCate( int categoryId)
        {
            if (categoryId == null)
            {
                return BadRequest(ModelState);
            }


            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }


            if(!_categoryRepository.DeleteCategory(categoryId))
            {
                ModelState.AddModelError("","Something went wrong with saving");
                return StatusCode(500, ModelState); 
            }
            return NoContent();
        }

    }
}