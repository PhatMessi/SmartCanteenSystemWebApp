// File: SCMS.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Threading.Tasks;

namespace SCMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);

            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Token = token });
        }
    }
}