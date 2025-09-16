// File: SCMS.Application/IMenuService.cs
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
        Task<MenuItem?> CreateMenuItemAsync(CreateMenuItemDto menuItemDto);
        Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemDto menuItemDto);
        Task<bool> DeleteMenuItemAsync(int id);
    }
}