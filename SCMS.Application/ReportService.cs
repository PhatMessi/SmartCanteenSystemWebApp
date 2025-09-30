// File: SCMS.Application/ReportService.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain;
using SCMS.Domain.DTOs;
using SCMS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        // --- Hàm GetSalesSummaryAsync không thay đổi, giữ nguyên ---
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

        // --- Hàm GetStudentSpendingReportAsync đã được sửa ---
        public async Task<StudentSpendingReportDto> GetStudentSpendingReportAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return new StudentSpendingReportDto { StartDate = startDate, EndDate = endDate };
            }

            var targetStudentIds = new List<int>();

            // FIX 1: Sửa 'user.Role.Name' thành 'user.Role.RoleName'
            if (user.Role.RoleName == "Student")
            {
                targetStudentIds.Add(user.UserId);
            }
            // FIX 1: Sửa 'user.Role.Name' thành 'user.Role.RoleName'
            else if (user.Role.RoleName == "Parent")
            {
                var childIds = await _context.Users
                                    .Where(u => u.ParentId == userId)
                                    .Select(u => u.UserId)
                                    .ToListAsync();
                targetStudentIds.AddRange(childIds);
            }

            if (!targetStudentIds.Any())
            {
                return new StudentSpendingReportDto { StartDate = startDate, EndDate = endDate };
            }

            var relevantStatuses = new[] { "Preparing", "Ready for Pickup", "Completed", "Paid" };

            var orders = await _context.Orders
                .Where(o => targetStudentIds.Contains(o.UserId) &&
                            o.OrderDate >= startDate && o.OrderDate < endDate.AddDays(1) &&
                            relevantStatuses.Contains(o.Status))
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var report = new StudentSpendingReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalOrders = orders.Count,
                TotalSpent = orders.Sum(o => o.TotalPrice),
                Orders = orders.Select(o => new StudentSpendingReportDto.OrderSummary
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    Items = o.OrderItems.Select(oi => new StudentSpendingReportDto.ItemDetail
                    {
                        ItemName = oi.MenuItem.Name,
                        Quantity = oi.Quantity,
                        // FIX 2: Sửa 'oi.Price' thành 'oi.PriceAtTimeOfOrder'
                        Price = oi.PriceAtTimeOfOrder
                    }).ToList()
                }).ToList()
            };

            return report;
        }
    }
}