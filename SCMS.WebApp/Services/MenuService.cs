// File: SCMS.WebApp/Services/MenuService.cs
using Microsoft.AspNetCore.Components.Forms;
using SCMS.Domain;
using SCMS.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SCMS.WebApp.Services
{
    public class MenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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

        public async Task<List<Category>?> GetCategoriesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Category>>("api/menu/categories");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                return new List<Category>();
            }
        }

        // THÊM MỚI: Phương thức để upload ảnh và tạo món ăn
        public async Task<MenuItem?> CreateMenuItemWithImageAsync(CreateMenuItemDto menuItem, IBrowserFile? imageFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(menuItem.Name), "Name");
            content.Add(new StringContent(menuItem.Description ?? ""), "Description");
            content.Add(new StringContent(menuItem.Price.ToString()), "Price");
            content.Add(new StringContent(menuItem.InventoryQuantity.ToString()), "InventoryQuantity");
            content.Add(new StringContent(menuItem.CategoryId.ToString()), "CategoryId");

            content.Add(new StringContent(menuItem.ImageUrl ?? ""), "ImageUrl");

            if (imageFile != null)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5)); // Giới hạn 5MB
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.Name);
            }

            var response = await _httpClient.PostAsync("api/Menu", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MenuItem>();
            }
            return null;
        }

        // Cập nhật phương thức Update để có thể gửi ảnh
        public async Task<bool> UpdateMenuItemWithImageAsync(int id, UpdateMenuItemDto menuItem, IBrowserFile? imageFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(menuItem.Name), "Name");
            content.Add(new StringContent(menuItem.Description ?? ""), "Description");
            content.Add(new StringContent(menuItem.Price.ToString()), "Price");
            content.Add(new StringContent(menuItem.InventoryQuantity.ToString()), "InventoryQuantity");
            content.Add(new StringContent(menuItem.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(menuItem.IsAvailable.ToString()), "IsAvailable");

            content.Add(new StringContent(menuItem.ImageUrl ?? ""), "ImageUrl");

            if (imageFile != null)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5)); // Giới hạn 5MB
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.Name);
            }

            var response = await _httpClient.PutAsync($"api/Menu/{id}", content);
            return response.IsSuccessStatusCode;
        }


        public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<MenuItem>($"api/Menu/{id}");
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Menu/{id}");
            return response.IsSuccessStatusCode;
        }
        public string GetApiBaseUrl()
        {
            // Trả về địa chỉ gốc của API, loại bỏ dấu / ở cuối nếu có
            return _httpClient.BaseAddress.ToString().TrimEnd('/');
        }
    }
}