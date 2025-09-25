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
        // Foreign key cho tài khoản phụ huynh
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual User Parent { get; set; }

        // Foreign key cho tài khoản giáo viên chủ nhiệm
        public int? HeadTeacherId { get; set; }
        [ForeignKey("HeadTeacherId")]
        public virtual User HeadTeacher { get; set; }

        // Một phụ huynh có thể liên kết với nhiều học sinh
        public virtual ICollection<User> LinkedStudents { get; set; } = new List<User>();

        // Quan hệ một-một với Wallet
        public virtual Wallet Wallet { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public bool MustChangePassword { get; set; } = false;
    }
}