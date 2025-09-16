// File: SCMS.WebApp/Services/TokenService.cs
namespace SCMS.WebApp.Services
{
    // Service này sẽ được tạo mới cho mỗi phiên kết nối của người dùng
    // và dùng để lưu trữ token của họ trên server.
    public class TokenService
    {
        public string? Token { get; set; }
    }
}