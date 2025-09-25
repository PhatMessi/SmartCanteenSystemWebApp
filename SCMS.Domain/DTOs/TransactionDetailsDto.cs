using System;
namespace SCMS.Domain.DTOs
{
    public class TransactionDetailsDto
    {
        public int TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int? OrderId { get; set; }
    }
}
