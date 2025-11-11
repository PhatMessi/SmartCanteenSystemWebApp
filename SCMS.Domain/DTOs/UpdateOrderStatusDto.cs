// File: SCMS.Application/DTOs/UpdateOrderStatusDto.cs
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain.DTOs
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}