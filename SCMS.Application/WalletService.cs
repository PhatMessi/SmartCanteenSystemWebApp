// File: SCMS.Application/WalletService.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain;
using SCMS.Domain.DTOs;
using SCMS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;

        public WalletService(ApplicationDbContext context, UserService userService, NotificationService notificationService)
        {
            _context = context;
            _userService = userService;
            _notificationService = notificationService;
        }

        private async Task<Wallet> GetOrCreateWalletAsync(int userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
            {
                wallet = new Wallet { UserId = userId, Balance = 0 };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }
            return wallet;
        }

        public async Task<WalletDto> GetWalletByUserIdAsync(int userId)
        {
            var wallet = await GetOrCreateWalletAsync(userId);
            return new WalletDto { Balance = wallet.Balance };
        }

        public async Task<List<TransactionDetailsDto>> GetTransactionHistoryAsync(int userId)
        {
            var wallet = await GetOrCreateWalletAsync(userId);
            return await _context.Transactions
                .Where(t => t.WalletId == wallet.WalletId)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionDetailsDto
                {
                    TransactionId = t.TransactionId,
                    Type = t.Type.ToString(),
                    Amount = t.Amount,
                    Status = t.Status,
                    TransactionDate = t.TransactionDate,
                    Description = t.Description,
                    OrderId = t.OrderId
                })
                .ToListAsync();
        }

        public async Task<bool> TopUpAsync(int userId, decimal amount, int? fromUserId = null)
        {
            if (amount <= 0) return false;

            var wallet = await GetOrCreateWalletAsync(userId);
            wallet.Balance += amount;

            var description = fromUserId.HasValue
                ? $"Nạp tiền từ tài khoản phụ huynh (ID: {fromUserId.Value})"
                : "Tự nạp tiền.";

            _context.Transactions.Add(new Transaction
            {
                WalletId = wallet.WalletId,
                Type = TransactionType.TopUp,
                Amount = amount,
                Status = "Success",
                TransactionDate = DateTime.UtcNow,
                Description = description
            });

            var success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _notificationService.CreateNotificationAsync(userId, $"Bạn đã nạp thành công {amount:N0}đ vào ví.", "/wallet");
            }
            return success;
        }

        public async Task<bool> ProcessPaymentAsync(int userId, int orderId, decimal amount)
        {
            var wallet = await GetOrCreateWalletAsync(userId);
            if (wallet.Balance < amount)
            {
                return false; // Không đủ tiền
            }

            wallet.Balance -= amount;
            _context.Transactions.Add(new Transaction
            {
                WalletId = wallet.WalletId,
                OrderId = orderId,
                Type = TransactionType.Payment,
                Amount = amount,
                Status = "Success",
                TransactionDate = DateTime.UtcNow,
                Description = $"Thanh toán cho đơn hàng #{orderId}"
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RefundAsync(int orderId, decimal amountToRefund, string description)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || amountToRefund <= 0) return false;

            var wallet = await GetOrCreateWalletAsync(order.UserId);
            wallet.Balance += amountToRefund;

            _context.Transactions.Add(new Transaction
            {
                WalletId = wallet.WalletId,
                OrderId = orderId,
                Type = TransactionType.Refund,
                Amount = amountToRefund, // Sử dụng số tiền được truyền vào
                Status = "Success",
                TransactionDate = DateTime.UtcNow,
                Description = description // Sử dụng mô tả được truyền vào
            });

            var success = await _context.SaveChangesAsync() > 0;

            if (success)
            {
                await _notificationService.CreateNotificationAsync(order.UserId, $"Bạn đã được hoàn {amountToRefund:N0}đ vào ví.", "/wallet");
            }

            return success;
        }
        public async Task<(List<TransactionDetailsDto>? History, string? ErrorMessage)> GetTransactionHistoryForStudentAsync(int studentId, int parentId)
        {
            // Bước kiểm tra bảo mật quan trọng
            var isLinked = await _userService.IsParentOfStudentAsync(parentId, studentId);
            if (!isLinked)
            {
                return (null, "Bạn không có quyền xem lịch sử giao dịch của học sinh này.");
            }

            var wallet = await GetOrCreateWalletAsync(studentId);
            var history = await _context.Transactions
                .Where(t => t.WalletId == wallet.WalletId)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionDetailsDto
                {
                    TransactionId = t.TransactionId,
                    Type = t.Type.ToString(),
                    Amount = t.Amount,
                    Status = t.Status,
                    TransactionDate = t.TransactionDate,
                    Description = t.Description,
                    OrderId = t.OrderId
                })
                .ToListAsync();

            return (history, null);
        }
    }
}