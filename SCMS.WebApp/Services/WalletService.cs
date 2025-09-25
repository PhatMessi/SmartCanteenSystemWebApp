// File: SCMS.WebApp/Services/WalletService.cs
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SCMS.Domain.DTOs; // Cần using DTOs để sử dụng WalletDto
using System.Collections.Generic;

namespace SCMS.WebApp.Services
{
    public class WalletService
    {
        private readonly HttpClient _httpClient;

        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WalletDto?> GetWalletAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<WalletDto>("api/wallet");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<List<TransactionDetailsDto>?> GetTransactionHistoryAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<TransactionDetailsDto>>("api/wallet/history");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<(bool Success, string Message)> TopUpAsync(TopUpRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/wallet/topup", request);
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (response.IsSuccessStatusCode, result?["message"] ?? "Lỗi không xác định.");
        }
        public async Task<List<TransactionDetailsDto>?> GetStudentTransactionHistoryAsync(int studentId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<TransactionDetailsDto>>($"api/wallet/student/{studentId}/history");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}