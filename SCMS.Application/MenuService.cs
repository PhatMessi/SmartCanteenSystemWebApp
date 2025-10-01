// File: SCMS.Application/MenuService.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain;
using SCMS.Domain.DTOs;
using SCMS.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SCMS.Application
{
    // LƯU Ý: ĐÃ XÓA BỎ PHẦN ĐỊNH NGHĨA INTERFACE BỊ TRÙNG Ở ĐÂY

    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Category>?> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<MenuItem>?> GetMenuItemsAsync(string? searchTerm, int? categoryId)
        {
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

        public async Task<MenuItem?> CreateMenuItemAsync(CreateMenuItemDto menuItemDto, IFormFile? imageFile)
        {
            var category = await _context.Categories.FindAsync(menuItemDto.CategoryId);
            if (category == null)
            {
                return null;
            }

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price,
                InventoryQuantity = menuItemDto.InventoryQuantity,
                CategoryId = menuItemDto.CategoryId,
                IsAvailable = true
            };

            if (imageFile is not null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "menu");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                menuItem.ImageUrl = $"/images/menu/{uniqueFileName}";
            }
            // Ưu tiên 2: Nếu không có file upload, sử dụng URL từ DTO
            else if (!string.IsNullOrWhiteSpace(menuItemDto.ImageUrl))
            {
                // Có thể thêm bước kiểm tra URL hợp lệ ở đây nếu muốn
                menuItem.ImageUrl = menuItemDto.ImageUrl;
            }
            // Ưu tiên 3: Nếu cả hai đều không có, dùng ảnh mặc định
            else
            {
                menuItem.ImageUrl = "/images/default-food.png";
            }

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemDto menuItemDto, IFormFile? imageFile)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            // SỬA LỖI: Đảm bảo biến oldImageUrl được khai báo ở đây
            var oldImageUrl = menuItem.ImageUrl;

            // Cập nhật các thuộc tính khác
            menuItem.Name = menuItemDto.Name;
            menuItem.Description = menuItemDto.Description;
            menuItem.Price = menuItemDto.Price;
            menuItem.InventoryQuantity = menuItemDto.InventoryQuantity;
            menuItem.CategoryId = menuItemDto.CategoryId;
            menuItem.IsAvailable = menuItemDto.IsAvailable;

            // Ưu tiên 1: Xử lý file mới được upload
            if (imageFile is not null && imageFile.Length > 0)
            {
                // Xóa file ảnh cũ nếu nó là file trên server
                DeleteOldImageFile(oldImageUrl);

                // Lưu file ảnh mới
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "menu");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                menuItem.ImageUrl = $"/images/menu/{uniqueFileName}"; // Cập nhật URL mới
            }
            // Ưu tiên 2: Xử lý URL mới được dán vào
            else if (!string.IsNullOrWhiteSpace(menuItemDto.ImageUrl) && menuItemDto.ImageUrl != oldImageUrl)
            {
                // Nếu URL mới khác URL cũ, tức là người dùng đã thay đổi nó
                // Xóa file ảnh cũ nếu nó là file trên server
                DeleteOldImageFile(oldImageUrl);

                // Cập nhật URL mới từ DTO
                menuItem.ImageUrl = menuItemDto.ImageUrl;
            }

            if (!menuItem.IsAvailable)
            {
                menuItem.InventoryQuantity = 0;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private void DeleteOldImageFile(string? imageUrl)
        {
            // THÊM DÒNG KIỂM TRA NÀY
            if (string.IsNullOrEmpty(_webHostEnvironment.WebRootPath))
            {
                // Nếu không tìm thấy wwwroot thì không làm gì cả để tránh lỗi
                return;
            }

            if (string.IsNullOrEmpty(imageUrl) || imageUrl.StartsWith("http"))
            {
                // Không xóa nếu URL rỗng hoặc là link từ bên ngoài
                return;
            }

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            DeleteOldImageFile(menuItem.ImageUrl);

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}