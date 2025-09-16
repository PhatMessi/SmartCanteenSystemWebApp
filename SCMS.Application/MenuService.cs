// File: SCMS.Application/MenuService.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain;
using SCMS.Domain.DTOs;
using SCMS.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;

        public MenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>?> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<MenuItem>?> GetMenuItemsAsync(string? searchTerm, int? categoryId)
        {
            // Thêm Include(m => m.Category) để lấy thông tin Category nếu cần
            var query = _context.MenuItems.Include(m => m.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(m => m.Name.Contains(searchTerm));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(m => m.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
        {
            return await _context.MenuItems.Include(m => m.Category).FirstOrDefaultAsync(m => m.ItemId == id);
        }

        public async Task<MenuItem?> CreateMenuItemAsync(CreateMenuItemDto menuItemDto)
        {
            var category = await _context.Categories.FindAsync(menuItemDto.CategoryId);
            if (category == null)
            {
                // Xử lý trường hợp không tìm thấy category
                return null;
            }

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price,
                ImageUrl = menuItemDto.ImageUrl,
                InventoryQuantity = menuItemDto.InventoryQuantity,
                CategoryId = menuItemDto.CategoryId,
                IsAvailable = true
            };
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemDto menuItemDto)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            menuItem.Name = menuItemDto.Name;
            menuItem.Description = menuItemDto.Description;
            menuItem.Price = menuItemDto.Price;
            menuItem.ImageUrl = menuItemDto.ImageUrl;
            menuItem.InventoryQuantity = menuItemDto.InventoryQuantity;
            menuItem.CategoryId = menuItemDto.CategoryId;
            menuItem.IsAvailable = menuItemDto.IsAvailable;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}