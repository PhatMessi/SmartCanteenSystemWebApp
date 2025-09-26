using SCMS.Domain;
using SCMS.Domain.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic; 

namespace SCMS.WebApp.Services
{

    // Các lớp này dùng để đóng gói kết quả trả về từ service, giúp UI dễ dàng xử lý
    // và hiển thị thông báo chi tiết cho người dùng.

    // Chứa kết quả chung cho các thao tác trả về message (thành công/thất bại)
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// Chứa kết quả cho việc đặt hàng mới
    public class PlaceOrderResult : OperationResult
    {
        public Order? CreatedOrder { get; set; }
    }

    /// Chứa kết quả cho việc cập nhật đơn hàng
    public class UpdateOrderResult : OperationResult
    {
        public Order? UpdatedOrder { get; set; }
    }
    
    /// Dùng để đọc cấu trúc lỗi chung từ API (e.g., { "message": "..." })
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
    }


    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly NotificationService _notificationService;

        public OrderService(HttpClient httpClient, NotificationService notificationService)
        {
            _httpClient = httpClient;
            _notificationService = notificationService;
        }


        /// Đặt một đơn hàng mới. Trả về kết quả chi tiết.
        public async Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequestDto orderDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Orders", orderDto);

                if (response.IsSuccessStatusCode)
                {
                    await _notificationService.RefreshUnreadCountAsync();
                    var createdOrder = await response.Content.ReadFromJsonAsync<Order>();
                    return new PlaceOrderResult { Success = true, CreatedOrder = createdOrder, Message = "Đặt hàng thành công!" };
                }
                else
                {
                    // Đọc lỗi từ ErrorHandlingMiddleware hoặc BadRequest
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return new PlaceOrderResult { Success = false, Message = error?.Message ?? "Đặt hàng thất bại." };
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi kết nối mạng, etc.
                return new PlaceOrderResult { Success = false, Message = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

        /// Cập nhật một đơn hàng đang chờ thanh toán. Trả về kết quả chi tiết.
        public async Task<UpdateOrderResult> UpdateOrderAsync(int orderId, PlaceOrderRequestDto orderDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Orders/{orderId}", orderDto);

                if (response.IsSuccessStatusCode)
                {
                    var updatedOrder = await response.Content.ReadFromJsonAsync<Order>();
                    return new UpdateOrderResult { Success = true, UpdatedOrder = updatedOrder, Message = "Cập nhật đơn hàng thành công!" };
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return new UpdateOrderResult { Success = false, Message = error?.Message ?? "Cập nhật thất bại." };
                }
            }
            catch (Exception ex)
            {
                return new UpdateOrderResult { Success = false, Message = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

        /// Xác nhận thanh toán cho một đơn hàng. Trả về kết quả chi tiết.
        public async Task<OperationResult> ConfirmOrderPaymentAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/Orders/{orderId}/confirm-payment", null);
                
                // API trả về message cho cả trường hợp thành công và thất bại
                var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(); // Dùng chung ErrorResponse vì cấu trúc JSON { "message": "..." } là như nhau
                if (response.IsSuccessStatusCode)
                {
                    // --- BẮT ĐẦU THAY ĐỔI 4 ---
                    await _notificationService.RefreshUnreadCountAsync();
                    // --- KẾT THÚC THAY ĐỔI 4 ---
                }
                return new OperationResult
                {
                    Success = response.IsSuccessStatusCode,
                    Message = result?.Message ?? "Thao tác không nhận được phản hồi."
                };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

        /// Hủy một đơn hàng đã thanh toán. Trả về kết quả chi tiết.
        public async Task<OperationResult> CancelOrderAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/Orders/{orderId}/cancel", null);
                
                var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                if (response.IsSuccessStatusCode)
                {
                    // --- BẮT ĐẦU THAY ĐỔI 5 ---
                    await _notificationService.RefreshUnreadCountAsync();
                    // --- KẾT THÚC THAY ĐỔI 5 ---
                }
                return new OperationResult
                {
                    Success = response.IsSuccessStatusCode,
                    Message = result?.Message ?? "Thao tác không nhận được phản hồi."
                };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Lỗi hệ thống: {ex.Message}" };
            }
        }



        public async Task<List<Order>?> GetMyOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("api/orders/my-orders");
        }

        public async Task<List<Order>?> GetProcessableOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("api/Orders");
        }

        public async Task<List<Order>?> GetOrderHistoryAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("api/Orders/history");
        }

        public async Task<(bool Success, string Message, Order? Order)> ProgressOrderAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/orders/{orderId}/progress", null);
                if (response.IsSuccessStatusCode)
                {
                    var updatedOrder = await response.Content.ReadFromJsonAsync<Order>();
                    return (true, "Cập nhật thành công", updatedOrder);
                }
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, error?.Message ?? "Cập nhật thất bại.", null);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}", null);
            }
        }

        public async Task<OperationResult> RejectOrderAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/orders/{orderId}/reject", null);
                var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return new OperationResult
                {
                    Success = response.IsSuccessStatusCode,
                    Message = result?.Message ?? "Thao tác không nhận được phản hồi."
                };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

    }
}