// File: SCMS.Domain/Notification.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Domain
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; } // ID của người dùng nhận thông báo

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false; // Mặc định là chưa đọc

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Link { get; set; } // Đường dẫn khi click vào, vd: "/my-orders/123"
    }
}