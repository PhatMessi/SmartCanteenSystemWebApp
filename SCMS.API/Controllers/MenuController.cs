// File: SCMS.API/Controllers/MenuController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using SCMS.Domain.DTOs;
using System.Threading.Tasks;

namespace SCMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [AllowAnonymous] // Cho phép xem menu mà không cần đăng nhập
        public async Task<IActionResult> Get([FromQuery] string? searchTerm, [FromQuery] int? categoryId)
        {
            var menuItems = await _menuService.GetMenuItemsAsync(searchTerm, categoryId);
            return Ok(menuItems);
        }

        // Endpoint mới để lấy danh sách categories
        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _menuService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var menuItem = await _menuService.GetMenuItemByIdAsync(id);
            if (menuItem == null) return NotFound();
            return Ok(menuItem);
        }

        [HttpPost]
        [Authorize(Roles = "CanteenManager,SystemAdmin")] // Sử dụng tên vai trò
        public async Task<IActionResult> Create(CreateMenuItemDto menuItemDto)
        {
            var createdItem = await _menuService.CreateMenuItemAsync(menuItemDto);
            if (createdItem == null)
            {
                return BadRequest("Invalid CategoryId.");
            }
            return CreatedAtAction(nameof(GetById), new { id = createdItem.ItemId }, createdItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "CanteenManager,SystemAdmin")]
        public async Task<IActionResult> Update(int id, UpdateMenuItemDto menuItemDto)
        {
            var success = await _menuService.UpdateMenuItemAsync(id, menuItemDto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CanteenManager,SystemAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _menuService.DeleteMenuItemAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}