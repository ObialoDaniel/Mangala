using Mangala.Core.DTOs.Category;
using Mangala.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mangala.API.Extensions;

namespace Mangala.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult GetAllCategories()
        {
            var response = categoryService.GetAllCategories();
            return Ok(response);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto request)
        {
           var userId = User.GetUserId();
           var response = categoryService.CreateCategory(request, userId);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(int id)
        {
            var response = categoryService.DeleteCategory(id);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }
    }
}
