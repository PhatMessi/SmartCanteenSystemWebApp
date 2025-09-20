using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Foreign Key: Khóa ngoại trỏ đến bảng Role
        public int RoleId { get; set; }

        [ForeignKey("RoleId")] // Chỉ rõ RoleId là khóa ngoại cho Role
        public virtual Role Role { get; set; } // Navigation Property: Mỗi User có một Role

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public bool MustChangePassword { get; set; } = false;
    }
}