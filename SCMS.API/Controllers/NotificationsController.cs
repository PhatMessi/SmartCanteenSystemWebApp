// File: SCMS.API/Controllers/NotificationsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu người dùng phải đăng nhập
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // API để lấy danh sách thông báo chưa đọc
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }

        // API để lấy số lượng thông báo chưa đọc
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var count = await _notificationService.GetUnreadNotificationCountAsync(userId);
            return Ok(new { count });
        }

        // API để đánh dấu tất cả thông báo là đã đọc
        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _notificationService.MarkAllAsReadAsync(userId);
            if (!success)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}