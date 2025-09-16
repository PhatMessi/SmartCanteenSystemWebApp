// File: SCMS.WebApp/Services/AuthService.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SCMS.Domain.DTOs;
using System; // Thêm
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class LoginResult { public string? Token { get; set; } }

namespace SCMS.WebApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly TokenService _tokenService;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider, TokenService tokenService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _tokenService = tokenService;
        }

        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            if (!response.IsSuccessStatusCode) return false;

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
            var token = loginResult?.Token;
            if (string.IsNullOrWhiteSpace(token)) return false;

            // =============================================================
            // === CAMERA GIÁM SÁT SỐ 1: TẠI THỜI ĐIỂM ĐĂNG NHẬP ===
            // =============================================================
            Console.WriteLine("\n--- [AuthService] ĐANG NHAP THANH CoNG ---");
            Console.WriteLine("[AuthService] Đa nhan đuoc token tu API.");
            Console.WriteLine("[AuthService] -> Chuan bi nap token vao TokenService...");
            // =============================================================

            await _localStorage.SetItemAsync("authToken", token);
            _tokenService.Token = token;

            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
            return true;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _tokenService.Token = null;
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        }
    }
}