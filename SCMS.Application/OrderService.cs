// File: SCMS.Application/OrderService.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.DTOs;
using SCMS.Domain;
using SCMS.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrderAsync(PlaceOrderRequestDto orderDto, int userId)
        {
            var menuItemIds = orderDto.Items.Select(item => item.MenuItemId).ToList();
            var menuItems = await _context.MenuItems
                .Where(mi => menuItemIds.Contains(mi.ItemId))
                .ToDictionaryAsync(mi => mi.ItemId);

            // BƯỚC 1: KIỂM TRA TỒN KHO TRƯỚC KHI TÍNH TOÁN
            foreach (var item in orderDto.Items)
            {
                if (!menuItems.TryGetValue(item.MenuItemId, out var menuItem) || menuItem.InventoryQuantity < item.Quantity)
                {
                    // Nếu món ăn không tồn tại hoặc không đủ hàng, báo lỗi ngay lập tức
                    throw new InvalidOperationException($"Not enough stock for menu item ID {item.MenuItemId}.");
                }
            }

            // --- Nếu tất cả đều đủ hàng, tiếp tục xử lý ---

            decimal totalPrice = 0;
            foreach (var item in orderDto.Items)
            {
                // Chúng ta đã chắc chắn menuItems[item.MenuItemId] tồn tại
                totalPrice += menuItems[item.MenuItemId].Price * item.Quantity;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                Status = "Pending Payment"
            };

            order.OrderItems = orderDto.Items.Select(item => new OrderItem
            {
                ItemId = item.MenuItemId,
                Quantity = item.Quantity,
                PriceAtTimeOfOrder = menuItems[item.MenuItemId].Price
            }).ToList();

            // BƯỚC 2: TRỪ SỐ LƯỢNG TỒN KHO
            foreach (var item in orderDto.Items)
            {
                menuItems[item.MenuItemId].InventoryQuantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu cả đơn hàng mới và số lượng tồn kho đã cập nhật

            return order;
        }
        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems) // Lấy kèm chi tiết các món ăn
                .ThenInclude(oi => oi.MenuItem) // Lấy kèm thông tin tên món ăn
                .ToListAsync();
        }
        public async Task<Order?> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders
                .Include(o => o.User) // <-- THÊM DÒNG NÀY
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return null;
            }

            order.Status = newStatus;
            await _context.SaveChangesAsync();

            return order;
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate) // Sắp xếp đơn hàng mới nhất lên đầu
                .ToListAsync();
        }
        public async Task<Order?> ConfirmOrderPaymentAsync(int orderId, int userId)
        {
            // SỬA LỖI: Thêm .Include() và .ThenInclude() để lấy kèm chi tiết các món ăn
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return null; // Không tìm thấy đơn hàng
            }

            // KIỂM TRA BẢO MẬT: Đảm bảo người dùng đang thao tác đúng là chủ của đơn hàng
            if (order.UserId != userId)
            {
                throw new UnauthorizedAccessException("User is not authorized to confirm payment for this order.");
            }

            if (order.Status == "Pending Payment")
            {
                order.Status = "Preparing"; // Trạng thái mới sau khi thanh toán thành công
                await _context.SaveChangesAsync();
            }

            return order;
        }
        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            // Lấy thông tin đơn hàng và các món ăn kèm theo
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            // Kiểm tra bảo mật: user có phải chủ đơn hàng không và đơn hàng có đang chờ thanh toán không
            if (order.UserId != userId || order.Status != "Pending Payment")
            {
                return false;
            }

            // Lấy danh sách ID các món ăn để truy vấn một lần
            var menuItemIds = order.OrderItems.Select(oi => oi.ItemId).ToList();
            var menuItemsToUpdate = await _context.MenuItems
                                                  .Where(mi => menuItemIds.Contains(mi.ItemId))
                                                  .ToListAsync();

            // *** BƯỚC QUAN TRỌNG: HOÀN TRẢ LẠI SỐ LƯỢNG TỒN KHO ***
            foreach (var orderItem in order.OrderItems)
            {
                var menuItem = menuItemsToUpdate.FirstOrDefault(mi => mi.ItemId == orderItem.ItemId);
                if (menuItem != null)
                {
                    menuItem.InventoryQuantity += orderItem.Quantity;
                }
            }

            // Cập nhật trạng thái đơn hàng
            order.Status = "Cancelled";

            // Lưu tất cả thay đổi (cả trạng thái đơn hàng và tồn kho) vào database
            await _context.SaveChangesAsync();

            return true;
        }
        // Thêm phương thức này vào file SCMS.Application/OrderService.cs

        public async Task<List<Order>> GetProcessableOrdersAsync()
        {
            var processableStatuses = new[] { "Paid", "Preparing", "Ready for Pickup" };

            return await _context.Orders
                .Where(o => processableStatuses.Contains(o.Status))
                .Include(o => o.User) // <-- THÊM DÒNG NÀY
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();
        }
        // Thêm phương thức này vào file SCMS.Application/OrderService.cs

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User) // Lấy thông tin người đặt
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate) // Sắp xếp đơn hàng mới nhất lên đầu
                .ToListAsync();
        }
    }
}