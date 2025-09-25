// File: SCMS.Domain/DTOs/LinkParentRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain.DTOs
{
    public class LinkParentRequestDto
    {
        [Required]
        [EmailAddress]
        public string ParentEmail { get; set; }
    }
}