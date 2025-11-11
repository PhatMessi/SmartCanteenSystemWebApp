// File: SCMS.Domain/DTOs/StudentSpendingReportDto.cs
using System;
using System.Collections.Generic;

namespace SCMS.Domain.DTOs
{
    public class StudentSpendingReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalOrders { get; set; }
        public List<OrderSummary> Orders { get; set; } = new List<OrderSummary>();

        public class OrderSummary
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
            public string Status { get; set; } = string.Empty;
            public List<ItemDetail> Items { get; set; } = new List<ItemDetail>();
        }

        public class ItemDetail
        {
            public string ItemName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}