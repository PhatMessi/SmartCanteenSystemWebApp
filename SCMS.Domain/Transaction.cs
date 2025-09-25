// File: SCMS.Domain/Transaction.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Domain
{
    public enum TransactionType
    {
        Payment,      // Thanh toán đơn hàng
        TopUp,        // Nạp tiền
        Withdrawal,   // Rút tiền
        Refund        // Hoàn tiền (do hủy đơn)
    }

    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        // --- THAY ĐỔI: Gắn với Wallet thay vì Order ---
        public int WalletId { get; set; }
        [ForeignKey("WalletId")]
        public virtual Wallet Wallet { get; set; }

        // OrderId có thể null (ví dụ khi nạp/rút tiền)
        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [Required]
        public TransactionType Type { get; set; } // Sử dụng enum để rõ ràng hơn

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Success, Failed

        public DateTime TransactionDate { get; set; }

        [MaxLength(255)]
        public string Description { get; set; } // Mô tả thêm, vd: "Nạp tiền từ phụ huynh"
    }
}