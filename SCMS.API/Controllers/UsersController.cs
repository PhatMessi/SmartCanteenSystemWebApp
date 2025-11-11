// File: SCMS.API/Controllers/UsersController.cs
using Microsoft.AspNetCore.Authorization; // Giữ lại using này
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Security.Claims;
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

        [HttpGet("my-parent")]
        [Authorize] // Yêu cầu đăng nhập
        public async Task<IActionResult> GetMyParent()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var parentDetails = await _userService.GetLinkedParentAsync(studentId);
            if (parentDetails == null)
            {
                return NotFound();
            }
            return Ok(parentDetails);
        }

        [HttpPost("link-parent")]
        [Authorize]
        public async Task<IActionResult> LinkParent(LinkParentRequestDto request)
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _userService.LinkParentAsync(studentId, request.ParentEmail);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        [HttpPost("unlink-parent")]
        [Authorize]
        public async Task<IActionResult> UnlinkParent()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _userService.UnlinkParentAsync(studentId);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
        [HttpGet("my-students")]
        [Authorize]
        public async Task<IActionResult> GetMyStudents()
        {
            var parentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var students = await _userService.GetLinkedStudentsAsync(parentId);
            // Chỉ trả về các thông tin cơ bản, không trả về PasswordHash
            var result = students.Select(s => new { s.UserId, s.FullName, s.Email });
            return Ok(result);
        }
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return Ok(roles);
        }
    }
}