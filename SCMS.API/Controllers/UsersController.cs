// File: SCMS.API/Controllers/UsersController.cs
using Microsoft.AspNetCore.Authorization; // Giữ lại using này
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Threading.Tasks;

namespace SCMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // THÊM DÒNG NÀY ĐỂ MỞ QUYỀN TRUY CẬP
    // [Authorize(Roles = "SystemAdmin")] // VÔ HIỆU HÓA DÒNG NÀY
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            var newUser = await _userService.CreateUserAsync(userDto);
            if (newUser == null)
            {
                return BadRequest(new { message = "Email đã tồn tại." });
            }
            return CreatedAtAction(nameof(GetAll), new { id = newUser.UserId }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto userDto)
        {
            var success = await _userService.UpdateUserAsync(id, userDto);
            if (!success)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }
            return NoContent();
        }
    }
}