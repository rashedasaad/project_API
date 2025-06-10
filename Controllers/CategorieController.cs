

using Microsoft.AspNetCore.Authorization;

namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly IdataHelper<Categories> _rebo;

        public CategorieController(IdataHelper<Categories> rebo)
        {
            _rebo = rebo;
        }

        [HttpGet("ViewData")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _rebo.GetView();
            return Ok(categories);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewCategory([FromBody] CrudCatogryDto CatogryDto)
        {
            if (CatogryDto == null)
                return BadRequest("Invalid data.");


            var item = new Categories
            {
                Title = CatogryDto.Title,
                Description = CatogryDto.Description,
            };

             _rebo.Add(item);
            return CreatedAtAction(nameof(GetCategories), new { id = item.Id }, item);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CrudCatogryDto CatogryDto)
        {
            var findOrder = await _rebo.FindByid(id);

            if (findOrder == null)
                return NotFound($"Category with id {id} not found.");

            findOrder.Description = CatogryDto.Description;
            findOrder.Title = CatogryDto.Title;

             _rebo.Edite(findOrder);
            return Ok(findOrder);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var findOrder = await _rebo.FindByid(id);

            if (findOrder == null)
                return NotFound($"Category with id {id} not found.");

             _rebo.Remove(findOrder);
            return NoContent();
        }
    }



}
