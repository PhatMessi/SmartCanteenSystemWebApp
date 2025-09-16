// File: SCMS.WebApp/Services/CustomAuthStateProvider.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System; // Thêm
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic; // Thêm
using System.Linq; // Thêm

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

            _tokenService.Token = token;

            var claimsIdentity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwtAuthType");
            return new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
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

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
                if (roles != null)
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());
                        foreach (var parsedRole in parsedRoles) claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }
                }
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
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