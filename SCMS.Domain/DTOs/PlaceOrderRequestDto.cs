// File: SCMS.Application/DTOs/PlaceOrderRequestDto.cs
using System.Collections.Generic;

namespace SCMS.Domain.DTOs
{
    public class PlaceOrderRequestDto
    {
        // UserId đã được xóa
        public List<OrderItemDto> Items { get; set; }
        public DateTime? PickupTime { get; set; }
    }
}