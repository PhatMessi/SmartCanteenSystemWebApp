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

        // BIẾN MỚI: Lưu ID của đơn hàng đang được sửa
        public int? EditingOrderId { get; private set; }

        // PHƯƠNG THỨC MỚI: Tải một đơn hàng vào giỏ để chỉnh sửa
        public void LoadOrderForEditing(Order orderToEdit)
        {
            Items.Clear();
            EditingOrderId = orderToEdit.OrderId;

            // Nạp các món ăn từ đơn hàng cũ vào giỏ hàng
            Items.AddRange(orderToEdit.OrderItems.Select(oi => new OrderItemDto
            {
                MenuItemId = oi.MenuItem.ItemId,
                Quantity = oi.Quantity,
                Price = oi.PriceAtTimeOfOrder,
                MenuItemName = oi.MenuItem.Name
            }));

            NotifyStateChanged();
        }

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
            // Reset luôn trạng thái chỉnh sửa khi xóa giỏ hàng
            EditingOrderId = null;
            NotifyStateChanged();
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}