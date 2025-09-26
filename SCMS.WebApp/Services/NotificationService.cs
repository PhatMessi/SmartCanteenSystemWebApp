// File: SCMS.WebApp/Services/NotificationService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SCMS.Domain;


public class NotificationService
{
    private readonly HttpClient _httpClient;

    public NotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetUnreadCountAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<UnreadCountResponse>("api/notifications/unread/count");
            return response?.Count ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    // THÊM MỚI: Phương thức lấy danh sách chi tiết các thông báo chưa đọc
    public async Task<List<Notification>> GetUnreadNotificationsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Notification>>("api/notifications/unread") ?? new List<Notification>();
        }
        catch
        {
            return new List<Notification>();
        }
    }

    // THÊM MỚI: Phương thức để đánh dấu tất cả thông báo là đã đọc
    public async Task<bool> MarkAllAsReadAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/notifications/mark-all-as-read", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private class UnreadCountResponse
    {
        public int Count { get; set; }
    }
}