// File: SCMS.Application/IWalletService.cs
using SCMS.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCMS.Application
{
    public interface IWalletService
    {
        Task<WalletDto> GetWalletByUserIdAsync(int userId);
        Task<List<TransactionDetailsDto>> GetTransactionHistoryAsync(int userId);
        Task<bool> TopUpAsync(int userId, decimal amount, int? fromUserId = null);
        Task<bool> ProcessPaymentAsync(int userId, int orderId, decimal amount);
        Task<bool> RefundAsync(int orderId, decimal amountToRefund, string description);
        Task<(List<TransactionDetailsDto>? History, string? ErrorMessage)> GetTransactionHistoryForStudentAsync(int studentId, int parentId);
    }
}