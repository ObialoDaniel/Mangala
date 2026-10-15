using Mangala.Core.DTOs.Auth;
using Mangala.Core.DTOs.Register;
using Mangala.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace Mangala.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [EnableRateLimiting("LoginPolicy")]
        public IActionResult Login(LoginDto request)
        {
            var result = authService.Login(request);
            if (!result.IsSuccess)
                return Unauthorized(result);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            var result = await authService.Register(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            return Challenge(
                new AuthenticationProperties { RedirectUri = "/api/auth/google-callback" },
                new[] { GoogleDefaults.AuthenticationScheme });
        }


        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized();

            var email = result.Principal!.FindFirst(ClaimTypes.Email)?.Value;
            var fullName = result.Principal!.FindFirst(ClaimTypes.Name)?.Value ?? email!;

            if (email is null)
                return BadRequest("Google did not provide an email address.");

            var response = authService.LoginWithGoogle(email, fullName);
            return Ok(response);
        }
    }
}
