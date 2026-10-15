using Mangala.Core.DTOs.MenuItem;
using Mangala.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mangala.API.Extensions;

namespace Mangala.API.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController (IMenuService menuService): ControllerBase
    {
        [HttpGet("today")]
        public IActionResult GetTodaysMenu()
        {
            var response = menuService.GetTodaysMenu();
            return Ok(response);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMenuItem([FromBody] CreateMenuItemDto request)
        {
           var userId = User.GetUserId();

            var response = menuService.CreateMenuItem(request, userId);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMenuItem(int id)
        {
            var response = menuService.DeleteMenuItem(id);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateMenuItem(int id, [FromBody] UpdateMenuItemDto request)
        {
            var response = menuService.UpdateMenuItem(id, request);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }
    }
}
