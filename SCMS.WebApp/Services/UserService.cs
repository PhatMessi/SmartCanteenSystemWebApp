using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SCMS.Domain;
using SCMS.Domain.DTOs;

namespace SCMS.WebApp.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>?> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/users");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<User>>() : null;
        }

        public async Task<User?> CreateUserAsync(CreateUserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", userDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        // --- BẮT ĐẦU CODE MỚI ---

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/{userId}", userDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ParentLinkDetailsDto?> GetLinkedParentAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ParentLinkDetailsDto>("api/users/my-parent");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // Trả về null nếu API trả về 404 Not Found
            }
        }

        public async Task<(bool Success, string Message)> LinkParentAsync(LinkParentRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/link-parent", request);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Yêu cầu liên kết đã được gửi.");
            }

            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            string errorMessage = "Đã xảy ra lỗi không xác định.";
            if (error != null)
            {
                if (error.TryGetValue("message", out var messageObj))
                {
                    errorMessage = messageObj?.ToString() ?? errorMessage;
                }
                else if (error.TryGetValue("title", out var titleObj))
                {
                    errorMessage = titleObj?.ToString() ?? errorMessage;
                }
            }

            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)> UnlinkParentAsync()
        {
            var response = await _httpClient.PostAsync("api/users/unlink-parent", null);
            if (response.IsSuccessStatusCode)
            {
                // Sửa ở đây để trả về đúng thông báo từ API
                var successResult = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return (true, successResult?["message"] ?? "Đã gửi yêu cầu hủy liên kết.");
            }

            // --- SỬA LỖI JSON Ở ĐÂY ---
            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            string errorMessage = "Đã xảy ra lỗi không xác định.";
            if (error != null && error.TryGetValue("message", out var messageObj))
            {
                errorMessage = messageObj?.ToString() ?? errorMessage;
            }
            return (false, errorMessage);
        }
        public async Task<List<Role>?> GetAllRolesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Role>>("api/users/roles");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}