// File: SCMS.API/Controllers/WalletController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu đăng nhập để truy cập
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly UserService _userService;

        public WalletController(IWalletService walletService, UserService userService)
        {
            _walletService = walletService;
            _userService = userService;
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var userId = GetCurrentUserId();
            var wallet = await _walletService.GetWalletByUserIdAsync(userId);
            return Ok(wallet);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = GetCurrentUserId();
            var history = await _walletService.GetTransactionHistoryAsync(userId);
            return Ok(history);
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp(TopUpRequestDto dto)
        {
            var userId = GetCurrentUserId();
            var success = await _walletService.TopUpAsync(userId, dto.Amount);
            if (!success)
            {
                return BadRequest(new { message = "Nạp tiền thất bại." });
            }
            return Ok(new { message = "Nạp tiền thành công." });
        }

        // Endpoint này dành cho phụ huynh nạp tiền cho con
        [HttpPost("topup-for-student/{studentId}")]
        [Authorize] // Chỉ cần đăng nhập là được, không cần kiểm tra vai trò ở đây
        public async Task<IActionResult> TopUpForStudent(int studentId, TopUpRequestDto dto)
        {
            var parentId = GetCurrentUserId();

            // --- BƯỚC KIỂM TRA BẢO MẬT QUAN TRỌNG ---
            var isLinked = await _userService.IsParentOfStudentAsync(parentId, studentId);
            if (!isLinked)
            {
                return Forbid("Bạn không có quyền thực hiện hành động này trên tài khoản của học sinh.");
            }

            var success = await _walletService.TopUpAsync(studentId, dto.Amount, parentId);
            if (!success)
            {
                return BadRequest(new { message = "Nạp tiền cho học sinh thất bại." });
            }
            return Ok(new { message = "Nạp tiền cho học sinh thành công." });
        }
        [HttpGet("student/{studentId}/history")]
        [Authorize(Roles = "Parent")] // Chỉ Parent mới được gọi endpoint này
        public async Task<IActionResult> GetStudentHistory(int studentId)
        {
            var parentId = GetCurrentUserId();
            var result = await _walletService.GetTransactionHistoryForStudentAsync(studentId, parentId);

            if (result.ErrorMessage != null)
            {
                return Forbid(result.ErrorMessage);
            }

            return Ok(result.History);
        }
    }
}