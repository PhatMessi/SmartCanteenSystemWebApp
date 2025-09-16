// File: SCMS.WebApp/Services/AuthHeaderHandler.cs
using System; // Thêm
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SCMS.WebApp.Services
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public AuthHeaderHandler(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // =============================================================
            // === BẮT ĐẦU ĐOẠN CODE DEBUG - "CAMERA GIÁM SÁT" ===
            // =============================================================

            Console.WriteLine("\n--- [AuthHeaderHandler] BAT ĐAU KIEM TRA REQUEST ---");
            Console.WriteLine($"--> Request đen: {request.Method} {request.RequestUri}");

            var token = _tokenService.Token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                // In ra vài ký tự đầu để xác nhận có token
                var displayToken = token.Length > 20 ? $"{token.Substring(0, 20)}..." : token;
                Console.WriteLine($"[AuthHeaderHandler] Trang thai: OK! ĐA TiM THAY TOKEN.");
                Console.WriteLine($"[AuthHeaderHandler] Token: Bearer {displayToken}");
                Console.WriteLine("[AuthHeaderHandler] -> Se them token vao Authorization Header.");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                Console.WriteLine("[AuthHeaderHandler] Trang thai: LOI! TOKEN TRONG TOKENSERVICE LÀ RONG (NULL).");
                Console.WriteLine("[AuthHeaderHandler] -> Se gui request đi ma khong co Authorization Header.");
            }

            Console.WriteLine("--- [AuthHeaderHandler] KET THUC KIEM TRA ---\n");

            // =============================================================
            // === KẾT THÚC ĐOẠN CODE DEBUG ===
            // =============================================================

            return base.SendAsync(request, cancellationToken);
        }
    }
}