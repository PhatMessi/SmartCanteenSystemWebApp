// File: SCMS.WebApp/Services/CustomAuthStateProvider.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace SCMS.WebApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly TokenService _tokenService;

        public CustomAuthStateProvider(ILocalStorageService localStorage, TokenService tokenService)
        {
            _localStorage = localStorage;
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // =============================================================
            // === CAMERA GIÁM SÁT SỐ 2: TẠI THỜI ĐIỂM TẢI LẠI TRANG ===
            // =============================================================
            Console.WriteLine("\n--- [CustomAuthStateProvider] TAI LAI TRANG THAI ---");
            Console.WriteLine("[CustomAuthStateProvider] Đa đoc đuoc token tu LocalStorage.");
            Console.WriteLine("[CustomAuthStateProvider] -> Chuan bi nap token vao TokenService...");
            // =============================================================

            try
            {
                _tokenService.Token = token;
                var claimsIdentity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwtAuthType");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
            catch (Exception ex)
            {
                // Nếu có bất kỳ lỗi nào khi phân tích token (token hỏng, hết hạn,...)
                Console.WriteLine($"Lỗi phân tích token: {ex.Message}");

                // Xóa token không hợp lệ khỏi bộ nhớ
                await _localStorage.RemoveItemAsync("authToken");
                _tokenService.Token = null;

                // Trả về trạng thái chưa đăng nhập
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // --- Các hàm ParseClaimsFromJwt và ParseBase64WithoutPadding giữ nguyên ---
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return claims;

            // Xử lý vai trò (roles) một cách an toàn
            if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object? rolesValue) && rolesValue is JsonElement rolesElement)
            {
                if (rolesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }
                }
                else if (rolesElement.ValueKind == JsonValueKind.String)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rolesElement.ToString()));
                }
            }

            // Xử lý các claims khác
            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value != null)
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
                }
            }

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}