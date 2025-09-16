// File: SCMS.WebApp/Services/OrderService.cs
using SCMS.Domain;
using SCMS.Domain.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SCMS.WebApp.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Order?> PlaceOrderAsync(PlaceOrderRequestDto orderDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Orders", orderDto);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Order>() : null;
        }

        // ===== BẮT ĐẦU SỬA LỖI EXCEPTION TRÊN TOÀN BỘ FILE =====
        public async Task<List<Order>?> GetMyOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/orders/my-orders");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<Order>>() : null;
        }

        public async Task<List<Order>?> GetProcessableOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/Orders");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<Order>>() : null;
        }

        public async Task<List<Order>?> GetOrderHistoryAsync()
        {
            var response = await _httpClient.GetAsync("api/Orders/history");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<Order>>() : null;
        }
        // ===== KẾT THÚC SỬA LỖI EXCEPTION =====

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var statusDto = new UpdateOrderStatusDto { Status = newStatus };
            var response = await _httpClient.PutAsJsonAsync($"api/orders/{orderId}/status", statusDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Order>();
            }
            return null;
        }

        public async Task<bool> ConfirmPaymentAsync(int orderId)
        {
            var response = await _httpClient.PostAsync($"api/Orders/{orderId}/confirm-payment", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var response = await _httpClient.PostAsync($"api/Orders/{orderId}/cancel", null);
            return response.IsSuccessStatusCode;
        }
    }
}