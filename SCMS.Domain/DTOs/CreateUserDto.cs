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
        public int RoleId { get; set; }
    }
}