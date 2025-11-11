using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SCMS.Domain;
using SCMS.Domain.DTOs;

namespace SCMS.WebApp.Services
{
    // Lớp nội bộ đơn giản để giúp đọc message từ API
    internal class ApiResponseDto
    {
        public string Message { get; set; }
    }

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

        // --- SỬA LỖI TẠI ĐÂY ---
        public async Task<(bool Success, string Message)> LinkParentAsync(LinkParentRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/link-parent", request);

            if (response.IsSuccessStatusCode)
            {
                // Nếu thành công, đọc message thành công
                var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
                return (true, result?.Message ?? "Thao tác thành công.");
            }
            else
            {
                // Nếu thất bại, đọc message lỗi
                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
                    return (false, errorResult?.Message ?? "Yêu cầu không hợp lệ.");
                }
                catch (JsonException)
                {
                    // Nếu body lỗi không có trường message (ví dụ lỗi validation mặc định)
                    return (false, "Thông tin cung cấp không hợp lệ. Vui lòng kiểm tra lại.");
                }
            }
        }

        // --- SỬA LỖI TẠI ĐÂY ---
        public async Task<(bool Success, string Message)> UnlinkParentAsync()
        {
            var response = await _httpClient.PostAsync("api/users/unlink-parent", null);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
                return (true, result?.Message ?? "Hủy liên kết thành công.");
            }
            else
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
                return (false, errorResult?.Message ?? "Không thể hủy liên kết.");
            }
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