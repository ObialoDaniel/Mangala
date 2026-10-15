using Mangala.Core.Interfaces;
using Mangala.Core.DTOs.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Mangala.API.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {

        [HttpPost("create")]
        public IActionResult CreateRole([FromBody] CreateRoleDto request)
        {
            var response = roleService.CreateRole(request);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAllRoles()
        {
            var response = roleService.GetAllRoles();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
