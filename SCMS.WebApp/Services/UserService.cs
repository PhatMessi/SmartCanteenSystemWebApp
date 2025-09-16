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

        // --- KẾT THÚC CODE MỚI ---
    }
}