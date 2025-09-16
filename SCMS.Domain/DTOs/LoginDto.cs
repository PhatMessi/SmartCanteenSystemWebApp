// File: SCMS.Application/DTOs/LoginDto.cs
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}