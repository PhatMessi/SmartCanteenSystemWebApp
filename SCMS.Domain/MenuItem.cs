using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace SCMS.Domain
{
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int InventoryQuantity { get; set; }

        public bool IsAvailable { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [JsonIgnore] // Quan trọng: Tránh lỗi lặp vô hạn khi API serialize
        public virtual Category? Category { get; set; } // Cho phép Category có thể null

        // Navigation property
        [JsonIgnore]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}