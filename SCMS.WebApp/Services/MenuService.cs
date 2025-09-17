// File: SCMS.WebApp/Services/MenuService.cs
using SCMS.Domain;
using SCMS.Domain.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq; // Thêm using này

namespace SCMS.WebApp.Services
{
    public class MenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // --- PHIÊN BẢN MỚI CỦA GetMenuItemsAsync ---
        public async Task<List<MenuItem>?> GetMenuItemsAsync(string? searchTerm = null, int? categoryId = null)
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                parameters["searchTerm"] = searchTerm;
            }
            if (categoryId.HasValue && categoryId > 0)
            {
                parameters["categoryId"] = categoryId.Value.ToString();
            }

            var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={System.Net.WebUtility.UrlEncode(p.Value)}"));
            var requestUri = $"api/Menu?{queryString}";

            return await _httpClient.GetFromJsonAsync<List<MenuItem>>(requestUri);
        }

        // --- PHƯƠNG THỨC MỚI ĐƯỢC THÊM VÀO ---
        public async Task<List<Category>?> GetCategoriesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Category>>("api/menu/categories");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                return new List<Category>();
            }
        }

        // --- CÁC PHƯƠNG THỨC CŨ CHO VIỆC QUẢN LÝ (GIỮ NGUYÊN) ---
        public async Task<MenuItem?> CreateMenuItemAsync(CreateMenuItemDto menuItemDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Menu", menuItemDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MenuItem>();
            }
            return null;
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<MenuItem>($"api/Menu/{id}");
        }

        public async Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemDto menuItemDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Menu/{id}", menuItemDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Menu/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}