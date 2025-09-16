// File: SCMS.Application/ReportService.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.DTOs;
using SCMS.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var relevantStatuses = new[] { "Preparing", "Ready for Pickup", "Completed" };

            var ordersInDateRange = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate.AddDays(1) && relevantStatuses.Contains(o.Status))
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();

            if (!ordersInDateRange.Any())
            {
                return new SalesSummaryDto { StartDate = startDate, EndDate = endDate };
            }

            var topItems = ordersInDateRange
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.MenuItem.Name)
                .Select(g => new SalesSummaryDto.TopSellingItem
                {
                    ItemName = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(i => i.TotalQuantity)
                .Take(5)
                .ToList();

            return new SalesSummaryDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = ordersInDateRange.Sum(o => o.TotalPrice),
                TotalOrders = ordersInDateRange.Count,
                TopItems = topItems
            };
        }
    }
}