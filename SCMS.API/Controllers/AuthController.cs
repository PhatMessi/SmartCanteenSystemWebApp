// File: SCMS.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Added
using System.Security.Claims; // Added

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
            // === THAY ĐỔI START ===
            var result = await _userService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không hợp lệ." });
            }

            // Trả về một đối tượng JSON chứa token và cờ báo hiệu cần đổi mật khẩu
            return Ok(new { result.Token, result.MustChangePassword });
            // === THAY ĐỔI END ===
        }

        // BEGIN: ADDED CODE
        // HÃY SỬA THÀNH:
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            // Bỏ dòng lấy clientBaseUrl và gọi hàm mới không cần tham số này
            var success = await _userService.ForgotPasswordAsync(dto.Email);
            if (success)
            {
                return Ok(new { Message = "If an account with this email exists, a password reset link has been sent." });
            }
            return BadRequest("Could not process the request.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var success = await _userService.ResetPasswordAsync(dto);
            if (success)
            {
                return Ok(new { Message = "Password has been reset successfully." });
            }
            return BadRequest("Invalid or expired token.");
        }

        [Authorize] // This endpoint requires the user to be logged in
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            // Get the user ID from the token claims
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            var success = await _userService.ChangePasswordAsync(userId, dto);

            if (success)
            {
                return Ok(new { Message = "Password changed successfully." });
            }
            return BadRequest(new { Message = "Incorrect old password." });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Return a DTO instead of the full User object for security
            return Ok(new { user.FullName, user.Email, Role = user.Role.RoleName });
        }
        // END: ADDED CODE
    }
}