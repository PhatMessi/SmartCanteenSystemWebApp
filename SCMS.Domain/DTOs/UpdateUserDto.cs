using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public int RoleId { get; set; }

        // ===== BẮT ĐẦU SỬA LỖI =====
        // Thêm 2 thuộc tính này để nhận dữ liệu mật khẩu mới từ form

        // Mật khẩu có thể để trống (nếu người dùng không muốn đổi)
        public string? Password { get; set; }

        // Dùng để so sánh với mật khẩu ở trên
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }
        public string? ParentEmail { get; set; }
    }
}