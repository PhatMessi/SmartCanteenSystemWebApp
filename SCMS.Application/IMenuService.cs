// File: SCMS.Application/IMenuService.cs
using Microsoft.AspNetCore.Http;
using SCMS.Domain;
using SCMS.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public interface IMenuService
    {
        Task<List<MenuItem>?> GetMenuItemsAsync(string? searchTerm, int? categoryId);
        Task<List<Category>?> GetAllCategoriesAsync();
        Task<MenuItem?> GetMenuItemByIdAsync(int id);
        Task<bool> DeleteMenuItemAsync(int id);

        // THAY ĐỔI: Cập nhật chữ ký phương thức để bao gồm IFormFile
        Task<MenuItem?> CreateMenuItemAsync(CreateMenuItemDto menuItemDto, IFormFile? imageFile);
        Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemDto menuItemDto, IFormFile? imageFile);
    }
}