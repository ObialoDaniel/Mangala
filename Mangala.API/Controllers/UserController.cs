using Mangala.Core.DTOs.User;
using Mangala.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Mangala.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("GeneralPolicy")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] CreateUserDto request)
        {
            var response = userService.CreateUser(request);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var response = userService.GetAllUsers();
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var response = userService.GetUserById(id);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }
    }
}