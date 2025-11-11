using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain
{
    public class Role
    {
        [Key] // Đánh dấu đây là khóa chính
        public int RoleId { get; set; }

        [Required] // Bắt buộc phải có
        [MaxLength(50)] // Độ dài tối đa 50 ký tự
        public string RoleName { get; set; }

        // Navigation Property: Một vai trò có thể có nhiều người dùng
        public virtual ICollection<User> Users { get; set; }
    }
}