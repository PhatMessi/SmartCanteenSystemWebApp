// File: SCMS.WebApp/Services/TokenService.cs (PHIÊN BẢN ĐÃ SỬA LỖI)
namespace SCMS.WebApp.Services
{
    /// <summary>
    /// Service này là Singleton, chỉ có một nhiệm vụ duy nhất:
    /// Lưu giữ chuỗi token trong bộ nhớ để các service Scoped khác có thể truy cập.
    /// Nó KHÔNG được inject bất kỳ service nào khác vào constructor.
    /// </summary>
    public class TokenService
    {
        public string? Token { get; set; }

        // Hàm khởi tạo phải rỗng, không có tham số
        public TokenService()
        {
        }
    }
}