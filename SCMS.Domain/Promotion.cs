using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain
{
    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        public string Description { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}