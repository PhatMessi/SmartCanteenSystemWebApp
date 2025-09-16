// File: SCMS.Domain/Transaction.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Domain
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } // e.g., Success, Failed

        public DateTime PaymentDate { get; set; }
    }
}