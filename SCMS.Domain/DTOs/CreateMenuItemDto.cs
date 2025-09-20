using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain.DTOs
{
    public class CreateMenuItemDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int InventoryQuantity { get; set; }

        [Required] 
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
    }
}