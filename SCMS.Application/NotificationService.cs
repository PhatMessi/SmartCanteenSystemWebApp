// File: SCMS.Application/NotificationService.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain;
using SCMS.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy các thông báo chưa đọc của một user
        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // Đếm số thông báo chưa đọc
        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            return await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        // Tạo một thông báo mới
        public async Task CreateNotificationAsync(int userId, string message, string? link = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Link = link
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        // Đánh dấu tất cả là đã đọc
        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            if (!notifications.Any()) return true;

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}