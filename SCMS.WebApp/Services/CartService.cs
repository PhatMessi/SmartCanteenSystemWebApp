// File: SCMS.WebApp/Services/CartService.cs
using SCMS.Domain;
using SCMS.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SCMS.WebApp.Services
{
    public class CartService
    {
        public List<OrderItemDto> Items { get; private set; } = new();
        public event Action? OnChange;

        public void AddItem(MenuItem menuItem)
        {
            var existingItem = Items.FirstOrDefault(i => i.MenuItemId == menuItem.ItemId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Items.Add(new OrderItemDto
                {
                    MenuItemId = menuItem.ItemId,
                    Quantity = 1,
                    Price = menuItem.Price,
                    MenuItemName = menuItem.Name
                });
            }
            NotifyStateChanged();
        }

        public void RemoveItem(int menuItemId)
        {
            var item = Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                Items.Remove(item);
                NotifyStateChanged();
            }
        }

        public void UpdateQuantity(int menuItemId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    Items.Remove(item);
                }
                NotifyStateChanged();
            }
        }

        public void ClearCart()
        {
            Items.Clear();
            NotifyStateChanged();
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}