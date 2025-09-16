// File: SCMS.Application/DTOs/CreateUserDto.cs
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int RoleId { get; set; } // Ví dụ: 1 = Student, 2 = Staff, 3 = Manager
    }
}