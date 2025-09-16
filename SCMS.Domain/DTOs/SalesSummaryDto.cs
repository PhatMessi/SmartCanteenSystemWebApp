using System;
using System.Collections.Generic;

namespace SCMS.Domain.DTOs
{
    public class SalesSummaryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<TopSellingItem> TopItems { get; set; } = new List<TopSellingItem>();

        public class TopSellingItem
        {
            public string ItemName { get; set; }
            public int TotalQuantity { get; set; }
        }
    }
}