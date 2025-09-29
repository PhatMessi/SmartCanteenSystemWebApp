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
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var message = result?["message"] ?? "Đã xảy ra lỗi.";

            return (response.IsSuccessStatusCode, message);
        }

        public async Task<(bool Success, string Message)> UnlinkParentAsync()
        {
            var response = await _httpClient.PostAsync("api/users/unlink-parent", null);
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var message = result?["message"] ?? "Đã xảy ra lỗi.";

            return (response.IsSuccessStatusCode, message);
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
        public async Task<List<User>?> GetMyStudentsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<User>>("api/users/my-students");
            }
            catch (HttpRequestException)
            {
                return new List<User>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }
    }
}