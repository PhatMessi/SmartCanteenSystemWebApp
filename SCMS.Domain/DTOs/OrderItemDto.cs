// File: SCMS.Domain/DTOs/OrderItemDto.cs
namespace SCMS.Domain.DTOs
{
    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }      // Thêm: Giá tại thời điểm đặt hàng
        public string? MenuItemName { get; set; } // Thêm: Tên món ăn để hiển thị
    }
}