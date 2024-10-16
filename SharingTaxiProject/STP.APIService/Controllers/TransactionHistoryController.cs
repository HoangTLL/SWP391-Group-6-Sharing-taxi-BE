using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TransactionHistoryController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public TransactionHistoryController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserTransactionHistory(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            // Kiểm tra xem người dùng có tồn tại không
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            // Lấy ví của người dùng
            var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                return NotFound($"Wallet for user with ID {userId} not found.");
            }

            // Lấy lịch sử giao dịch
            var transactions = await _unitOfWork.TransactionRepository.GetByWalletIdAsync(wallet.Id);

            // Phân trang kết quả
            var paginatedTransactions = transactions
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Chuyển đổi kết quả thành DTO để trả về
            var transactionDtos = paginatedTransactions.Select(t => new
            {
                t.Id,
                t.Amount,
                t.TransactionType,
                t.Status,
                t.CreatedAt,
                t.ReferenceId
            }).ToList();

            // Tạo thông tin phân trang
            var totalItems = transactions.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return Ok(new
            {
                Transactions = transactionDtos,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            });
        }
        catch (Exception ex)
        {
            // Log lỗi ở đây nếu cần
            return StatusCode(500, $"An error occurred while retrieving the transaction history: {ex.Message}");
        }
    }
}