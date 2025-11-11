// File: SCMS.WebApp/Services/AuthService.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SCMS.Domain.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json.Serialization; // Thêm using này

// === THAY ĐỔI START ===

// Class này để ánh xạ trực tiếp với JSON trả về từ API
public class LoginApiResponse
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("mustChangePassword")]
    public bool MustChangePassword { get; set; }
}

// Class này là kết quả trả về cho các trang Blazor, đơn giản và rõ ràng
public class LoginResult
{
    public bool Success { get; set; }
    public bool MustChangePassword { get; set; }
}

// === THAY ĐỔI END ===


namespace SCMS.WebApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly TokenService _tokenService;
        private readonly CartService _cartService;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider, TokenService tokenService, CartService cartService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _tokenService = tokenService;
            _cartService = cartService;
        }

        // === THAY ĐỔI START ===
        // Thay đổi kiểu trả về từ Task<bool> thành Task<LoginResult>
        public async Task<LoginResult> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                // Nếu API trả về lỗi (vd: 401 Unauthorized), trả về kết quả thất bại
                return new LoginResult { Success = false, MustChangePassword = false };
            }

            // Đọc nội dung JSON trả về từ API
            var apiResponse = await response.Content.ReadFromJsonAsync<LoginApiResponse>();
            if (apiResponse == null || string.IsNullOrWhiteSpace(apiResponse.Token))
            {
                // Nếu không có token, coi như thất bại
                return new LoginResult { Success = false, MustChangePassword = false };
            }

            // Lưu token vào Local Storage và thông báo cho hệ thống
            await _localStorage.SetItemAsync("authToken", apiResponse.Token);
            _tokenService.Token = apiResponse.Token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiResponse.Token);
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();

            // Trả về kết quả thành công cùng với cờ mustChangePassword
            return new LoginResult
            {
                Success = true,
                MustChangePassword = apiResponse.MustChangePassword
            };
        }
        // === THAY ĐỔI END ===

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _tokenService.Token = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _cartService.ClearCart();
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        }

        // --- Các hàm còn lại giữ nguyên không thay đổi ---

        public async Task<HttpResponseMessage> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            return await _httpClient.PostAsJsonAsync("api/auth/forgot-password", dto);
        }

        public async Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordDto dto)
        {
            return await _httpClient.PostAsJsonAsync("api/auth/reset-password", dto);
        }

        public async Task<UserProfile?> GetProfileAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token)) return null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<UserProfile>("api/auth/profile");
        }

        public async Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token)) return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PostAsJsonAsync("api/auth/change-password", dto);
        }
    }

    // Class UserProfile giữ nguyên
    public class UserProfile
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}