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
        private readonly IWalletService _walletService;
        private readonly NotificationService _notificationService;

        public OrderService(ApplicationDbContext context, IWalletService walletService, NotificationService notificationService)
        {
            _context = context;
            _walletService = walletService;
            _notificationService = notificationService;
        }

        public async Task<Order> PlaceOrderAsync(PlaceOrderRequestDto orderDto, int userId)
        {
            var menuItemIds = orderDto.Items.Select(item => item.MenuItemId).ToList();
            var menuItems = await _context.MenuItems
                .Where(mi => menuItemIds.Contains(mi.ItemId))
                .ToDictionaryAsync(mi => mi.ItemId);

            // KIỂM TRA TỒN KHO
            foreach (var item in orderDto.Items)
            {
                if (!menuItems.TryGetValue(item.MenuItemId, out var menuItem) || menuItem.InventoryQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for menu item ID {item.MenuItemId}.");
                }
            }

            DateTime? pickupTimeToSave = null;
            if (orderDto.PickupTime.HasValue)
            {
                // Coi thời gian người dùng nhập là giờ địa phương và chuyển nó sang giờ UTC để lưu trữ
                pickupTimeToSave = orderDto.PickupTime.Value.ToUniversalTime();

                // KIỂM TRA THỜI GIAN ĐẶT TRƯỚC HỢP LỆ (so sánh với giờ UTC hiện tại)
                if (pickupTimeToSave.Value <= DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Pickup time must be in the future.");
                }
            }

            decimal totalPrice = orderDto.Items.Sum(item => menuItems[item.MenuItemId].Price * item.Quantity);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow, // Giữ nguyên vì đây là thời điểm chính xác
                TotalPrice = totalPrice,
                Status = "Pending Payment",
                // Sử dụng giá trị đã được chuyển đổi sang UTC để lưu
                PickupTime = pickupTimeToSave
            };

            order.OrderItems = orderDto.Items.Select(item => new OrderItem
            {
                ItemId = item.MenuItemId,
                Quantity = item.Quantity,
                PriceAtTimeOfOrder = menuItems[item.MenuItemId].Price
            }).ToList();

            foreach (var item in orderDto.Items)
            {
                menuItems[item.MenuItemId].InventoryQuantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        // ... các phương thức còn lại giữ nguyên không thay đổi ...
        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems) // Lấy kèm chi tiết các món ăn
                .ThenInclude(oi => oi.MenuItem) // Lấy kèm thông tin tên món ăn
                .ToListAsync();
        }
        public async Task<(bool Success, string Message, Order? Order)> ProgressOrderStatusAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return (false, "Không tìm thấy đơn hàng.", null);
            }

            // Xác định trạng thái tiếp theo dựa trên trạng thái hiện tại
            string nextStatus = order.Status switch
            {
                "Paid" => "Preparing",
                "Preparing" => "Ready for Pickup",
                "Ready for Pickup" => "Completed",
                _ => "" // Các trạng thái khác không có hành động tiến tới mặc định
            };

            if (string.IsNullOrEmpty(nextStatus))
            {
                return (false, $"Không thể chuyển tiếp đơn hàng từ trạng thái '{order.Status}'.", null);
            }

            order.Status = nextStatus;
            await _context.SaveChangesAsync();
            string notificationMessage = nextStatus switch
            {
                "Preparing" => $"Đơn hàng #{order.OrderId} của bạn đang được chuẩn bị.",
                "Ready for Pickup" => $"Đơn hàng #{order.OrderId} đã sẵn sàng! Mời bạn đến nhận.",
                "Completed" => $"Đơn hàng #{order.OrderId} đã hoàn tất. Cảm ơn bạn!",
                _ => ""
            };

            if (!string.IsNullOrEmpty(notificationMessage))
            {
                await _notificationService.CreateNotificationAsync(order.UserId, notificationMessage, "/my-orders/");
            }

            return (true, $"Đã cập nhật đơn hàng sang trạng thái '{nextStatus}'.", order);
        }

        public async Task<(bool Success, string Message)> RejectOrderAsync(int orderId, string rejectionReason)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return (false, "Không tìm thấy đơn hàng.");
            }

            if (order.Status != "Paid")
            {
                return (false, $"Không thể từ chối đơn hàng đang ở trạng thái '{order.Status}'.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Hoàn tiền 100% cho khách hàng
                var refundMessage = $"Hoàn tiền cho đơn hàng #{orderId} bị Canteen từ chối.";
                var refundSuccess = await _walletService.RefundAsync(order.OrderId, order.TotalPrice, refundMessage);
                if (!refundSuccess)
                {
                    await transaction.RollbackAsync();
                    return (false, "Xảy ra lỗi trong quá trình hoàn tiền.");
                }

                // 2. Hoàn trả lại số lượng tồn kho
                var orderItems = await _context.OrderItems.Where(oi => oi.OrderId == orderId).ToListAsync();
                var menuItemIds = orderItems.Select(oi => oi.ItemId).ToList();
                var menuItemsToUpdate = await _context.MenuItems
                                              .Where(mi => menuItemIds.Contains(mi.ItemId))
                                              .ToListAsync();
                foreach (var orderItem in orderItems)
                {
                    var menuItem = menuItemsToUpdate.FirstOrDefault(mi => mi.ItemId == orderItem.ItemId);
                    if (menuItem != null)
                    {
                        menuItem.InventoryQuantity += orderItem.Quantity;
                    }
                }

                // 3. Cập nhật trạng thái và LƯU LÝ DO TỪ CHỐI
                order.Status = "Cancelled";
                order.RejectionReason = rejectionReason; // <-- LƯU LÝ DO VÀO DB

                await _context.SaveChangesAsync();

                // 4. Gửi thông báo kèm lý do
                string notificationMessage = $"Đơn hàng #{order.OrderId} của bạn đã bị hủy vì lý do: '{rejectionReason}'. Tiền đã được hoàn lại.";
                await _notificationService.CreateNotificationAsync(order.UserId, notificationMessage, "/my-orders/");

                await transaction.CommitAsync();

                return (true, "Từ chối đơn hàng thành công và đã hoàn tiền cho khách.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return (false, "Lỗi hệ thống khi từ chối đơn hàng.");
            }
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
        public async Task<(bool Success, string Message)> ConfirmOrderPaymentAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return (false, "Không tìm thấy đơn hàng hoặc bạn không có quyền thực hiện thao tác này.");
            }

            if (order.Status != "Pending Payment")
            {
                return (false, $"Đơn hàng đang ở trạng thái '{order.Status}', không thể thanh toán.");
            }

            // BƯỚC QUAN TRỌNG: GỌI WALLET SERVICE ĐỂ XỬ LÝ THANH TOÁN
            var paymentSuccessful = await _walletService.ProcessPaymentAsync(userId, orderId, order.TotalPrice);

            if (!paymentSuccessful)
            {
                return (false, "Thanh toán thất bại. Số dư trong ví không đủ.");
            }

            // Nếu thanh toán thành công, cập nhật trạng thái đơn hàng
            // Dựa trên tài liệu, trạng thái sau khi thanh toán là "Paid" (Đã thanh toán)
            // Trạng thái "Preparing" chỉ nên được cập nhật bởi nhân viên Canteen.
            order.Status = "Paid";
            await _context.SaveChangesAsync();
            await _notificationService.CreateNotificationAsync(userId, $"Bạn đã thanh toán thành công cho đơn hàng #{order.OrderId}.", "/my-orders/");

            return (true, "Thanh toán đơn hàng thành công.");
        }
        public async Task<(bool Success, string Message)> CancelOrderAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return (false, "Không tìm thấy đơn hàng.");
            if (order.UserId != userId) return (false, "Bạn không có quyền hủy đơn hàng này.");

            // QUY TẮC CHUNG: Không được hủy nếu trạng thái không phải là "Paid" hoặc đã "Ready"
            if (order.Status == "Ready for Pickup")
            {
                return (false, "Không thể hủy vì đơn hàng đã sẵn sàng để nhận.");
            }

            if (order.Status != "Paid")
            {
                return (false, $"Không thể hủy đơn hàng với trạng thái '{order.Status}'. Đơn hàng chỉ có thể hủy khi đã thanh toán thành công.");
            }

            decimal refundPercentage;
            string successMessage;

            // KỊCH BẢN 1: ĐƠN ĐẶT TRƯỚC (có PickupTime)
            if (order.PickupTime.HasValue)
            {
                var timeUntilPickup = order.PickupTime.Value - DateTime.UtcNow;

                if (timeUntilPickup.TotalMinutes < 30)
                {
                    refundPercentage = 0.9m; // Hoàn 90%
                    successMessage = "Hủy đơn hàng thành công. Vì bạn hủy trong vòng 30 phút trước giờ nhận, phí hủy 10% đã được áp dụng. 90% giá trị đơn đã được hoàn vào ví.";
                }
                else
                {
                    refundPercentage = 1.0m; // Hoàn 100%
                    successMessage = "Hủy đơn hàng thành công. Toàn bộ giá trị đơn hàng đã được hoàn vào ví của bạn.";
                }
            }
            // KỊCH BẢN 2: ĐƠN ĐẶT NGAY (không có PickupTime) - Áp dụng logic hoàn tiền theo các mốc thời gian
            else
            {
                var timeSinceOrder = DateTime.UtcNow - order.OrderDate;

                if (timeSinceOrder.TotalMinutes <= 10)
                {
                    refundPercentage = 1.0m; // Hoàn 100%
                    successMessage = "Hủy đơn hàng thành công trong 10 phút đầu, đã hoàn lại 100% giá trị đơn hàng.";
                }
                else if (timeSinceOrder.TotalMinutes <= 15) // Từ phút 10 đến 15
                {
                    refundPercentage = 0.9m; // Hoàn 90%
                    successMessage = "Hủy đơn hàng thành công. Phí hủy 10% đã được áp dụng, 90% giá trị đơn hàng đã được hoàn vào ví.";
                }
                else if (timeSinceOrder.TotalMinutes <= 20) // Từ phút 15 đến 20
                {
                    refundPercentage = 0.2m; // Hoàn 20%
                    successMessage = "Hủy đơn hàng thành công. Phí hủy 80% đã được áp dụng, 20% giá trị đơn hàng đã được hoàn vào ví.";
                }
                else // Sau 20 phút
                {
                    return (false, "Đã quá 20 phút kể từ lúc đặt hàng, không thể hủy đơn.");
                }
            }

            // Xử lý hoàn tiền và cập nhật DB trong một transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Tính toán số tiền hoàn lại
                decimal refundAmount = order.TotalPrice * refundPercentage;
                string refundMessage = $"Hoàn tiền {refundPercentage:P0} cho đơn hàng bị hủy #{orderId}.";

                // 2. Hoàn tiền vào ví
                if (refundAmount > 0)
                {
                    var refundSuccess = await _walletService.RefundAsync(order.OrderId, refundAmount, refundMessage);
                    if (!refundSuccess)
                    {
                        await transaction.RollbackAsync();
                        return (false, "Hoàn tiền thất bại, vui lòng thử lại.");
                    }
                }

                // 3. Hoàn trả lại số lượng tồn kho
                var menuItemIds = order.OrderItems.Select(oi => oi.ItemId).ToList();
                var menuItemsToUpdate = await _context.MenuItems
                                                      .Where(mi => menuItemIds.Contains(mi.ItemId))
                                                      .ToListAsync();
                foreach (var orderItem in order.OrderItems)
                {
                    var menuItem = menuItemsToUpdate.FirstOrDefault(mi => mi.ItemId == orderItem.ItemId);
                    if (menuItem != null)
                    {
                        menuItem.InventoryQuantity += orderItem.Quantity;
                    }
                }

                // 4. Cập nhật trạng thái đơn hàng
                order.Status = "Cancelled";

                await _context.SaveChangesAsync();
                await _notificationService.CreateNotificationAsync(userId, successMessage, "/my-orders/");
                await transaction.CommitAsync();

                return (true, successMessage);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return (false, "Đã xảy ra lỗi hệ thống khi hủy đơn.");
            }
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
        public class UpdateOrderResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string ErrorCode { get; set; } // Mã lỗi để Controller xử lý
            public Order UpdatedOrder { get; set; }
        }

        public async Task<UpdateOrderResult> UpdatePendingOrderAsync(int orderId, int userId, PlaceOrderRequestDto updatedDto)
        {
            // Bắt đầu một transaction để đảm bảo tất cả các thao tác đều thành công hoặc thất bại cùng nhau
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

                if (order == null)
                {
                    return new UpdateOrderResult { Success = false, Message = "Không tìm thấy đơn hàng.", ErrorCode = "NOT_FOUND" };
                }

                if (order.Status != "Pending Payment")
                {
                    return new UpdateOrderResult { Success = false, Message = "Chỉ có thể chỉnh sửa đơn hàng ở trạng thái 'Pending Payment'.", ErrorCode = "INVALID_STATUS" };
                }

                // 1. Hoàn trả lại số lượng tồn kho của các món ăn CŨ
                var oldMenuItemIds = order.OrderItems.Select(oi => oi.ItemId).ToList();
                var oldMenuItems = await _context.MenuItems
                                                 .Where(mi => oldMenuItemIds.Contains(mi.ItemId))
                                                 .ToListAsync();
                foreach (var oldItem in order.OrderItems)
                {
                    var menuItem = oldMenuItems.FirstOrDefault(mi => mi.ItemId == oldItem.ItemId);
                    if (menuItem != null)
                    {
                        menuItem.InventoryQuantity += oldItem.Quantity;
                    }
                }

                // Xóa các món ăn cũ khỏi đơn hàng
                _context.OrderItems.RemoveRange(order.OrderItems);
                await _context.SaveChangesAsync(); // Áp dụng thay đổi tồn kho và xóa item

                // 2. Xử lý các món ăn MỚI
                var newMenuItemIds = updatedDto.Items.Select(i => i.MenuItemId).ToList();
                var newMenuItemsDict = await _context.MenuItems
                                                     .Where(mi => newMenuItemIds.Contains(mi.ItemId))
                                                     .ToDictionaryAsync(mi => mi.ItemId);

                decimal newTotalPrice = 0;
                var newOrderItems = new List<OrderItem>();

                foreach (var newItemDto in updatedDto.Items)
                {
                    if (!newMenuItemsDict.TryGetValue(newItemDto.MenuItemId, out var menuItem))
                    {
                        await transaction.RollbackAsync();
                        return new UpdateOrderResult { Success = false, Message = $"Món ăn với ID {newItemDto.MenuItemId} không tồn tại.", ErrorCode = "NOT_FOUND" };
                    }

                    if (menuItem.InventoryQuantity < newItemDto.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return new UpdateOrderResult { Success = false, Message = $"Không đủ số lượng cho món '{menuItem.Name}'.", ErrorCode = "OUT_OF_STOCK" };
                    }

                    // Trừ số lượng tồn kho mới
                    menuItem.InventoryQuantity -= newItemDto.Quantity;

                    newTotalPrice += menuItem.Price * newItemDto.Quantity;
                    newOrderItems.Add(new OrderItem
                    {
                        ItemId = newItemDto.MenuItemId,
                        Quantity = newItemDto.Quantity,
                        PriceAtTimeOfOrder = menuItem.Price
                    });
                }

                // 3. Cập nhật lại đơn hàng chính
                order.TotalPrice = newTotalPrice;
                order.OrderItems = newOrderItems;
                order.OrderDate = DateTime.UtcNow; // Cập nhật lại ngày giờ đặt hàng

                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Hoàn tất transaction

                return new UpdateOrderResult { Success = true, UpdatedOrder = order };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Ghi log lỗi `ex` ở đây
                return new UpdateOrderResult { Success = false, Message = "Đã xảy ra lỗi hệ thống khi cập nhật đơn hàng.", ErrorCode = "SERVER_ERROR" };
            }
        }
        public async Task AutoCancelUnpaidOrdersAsync()
        {
            // 1. Xác định mốc thời gian (10 phút trước)
            var tenMinutesAgo = DateTime.UtcNow.AddMinutes(-10);

            // 2. Tìm tất cả đơn hàng có trạng thái "Pending Payment" và được tạo trước mốc thời gian trên
            //    Sử dụng Include để lấy thông tin các món hàng (OrderItems) cần thiết cho việc hoàn kho
            var ordersToCancel = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Status == "Pending Payment" && o.OrderDate <= tenMinutesAgo)
                .ToListAsync();

            if (!ordersToCancel.Any())
            {
                return; // Không có đơn hàng nào cần hủy, kết thúc sớm
            }

            // 3. Hoàn trả lại số lượng tồn kho
            var allItemIds = ordersToCancel.SelectMany(o => o.OrderItems.Select(oi => oi.ItemId)).Distinct().ToList();
            var menuItemsToUpdate = await _context.MenuItems
                                                  .Where(mi => allItemIds.Contains(mi.ItemId))
                                                  .ToDictionaryAsync(mi => mi.ItemId);

            foreach (var order in ordersToCancel)
            {
                // 4. Cập nhật trạng thái và lý do hủy cho từng đơn hàng
                order.Status = "Cancelled";
                order.RejectionReason = "Tự động hủy do không thanh toán sau 10 phút.";

                foreach (var orderItem in order.OrderItems)
                {
                    if (menuItemsToUpdate.TryGetValue(orderItem.ItemId, out var menuItem))
                    {
                        menuItem.InventoryQuantity += orderItem.Quantity;
                    }
                }
            }

            // 5. Lưu tất cả các thay đổi vào cơ sở dữ liệu một lần
            await _context.SaveChangesAsync();
        }
    }
}