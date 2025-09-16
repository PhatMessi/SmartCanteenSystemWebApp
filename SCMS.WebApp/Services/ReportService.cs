// File: SCMS.WebApp/Services/ReportService.cs
using SCMS.Domain.DTOs;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace SCMS.WebApp.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;
        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SalesSummaryDto?> GetSalesSummaryAsync(DateTime startDate, DateTime endDate)
        {
            // Định dạng ngày tháng để truyền qua URL
            var startDateString = startDate.ToString("yyyy-MM-dd");
            var endDateString = endDate.ToString("yyyy-MM-dd");

            var requestUri = $"api/reports/sales-summary?startDate={startDateString}&endDate={endDateString}";

            var response = await _httpClient.GetAsync(requestUri);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<SalesSummaryDto>() : null;
        }
    }
}