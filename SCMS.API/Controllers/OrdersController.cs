using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Threading.Tasks;

namespace SCMS.API.Controllers
{
    [AllowAnonymous]
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequestDto orderDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString);
            var order = await _orderService.PlaceOrderAsync(orderDto, userId);
            return CreatedAtAction(nameof(PlaceOrder), new { id = order.OrderId }, order);
        }

        [HttpGet]
        //[Authorize(Roles = "CanteenStaff,CanteenManager,SystemAdmin")]
        public async Task<IActionResult> GetProcessableOrders()
        {
            var orders = await _orderService.GetProcessableOrdersAsync();
            return Ok(orders);
        }

        [HttpPut("{orderId}/status")]
        //[Authorize(Roles = "CanteenStaff,CanteenManager,SystemAdmin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, UpdateOrderStatusDto statusDto)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status);
            if (updatedOrder == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
            return Ok(updatedOrder);
        }

        [HttpGet("my-orders")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString);
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost("{orderId}/confirm-payment")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> ConfirmPayment(int orderId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var updatedOrder = await _orderService.ConfirmOrderPaymentAsync(orderId, userId);
            if (updatedOrder == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
            return Ok(updatedOrder);
        }

        [HttpPost("{orderId}/cancel")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _orderService.CancelOrderAsync(orderId, userId);
            if (!success)
            {
                return BadRequest("Không thể hủy đơn hàng.");
            }
            return Ok(new { message = "Hủy đơn hàng thành công." });
        }

        [HttpGet("history")]
        //[Authorize(Roles = "CanteenStaff,CanteenManager,SystemAdmin")]
        public async Task<IActionResult> GetOrderHistory()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}