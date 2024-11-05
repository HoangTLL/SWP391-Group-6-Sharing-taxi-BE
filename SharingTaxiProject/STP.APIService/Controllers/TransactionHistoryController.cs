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
            // BƯỚC 1: Kiểm tra người dùng
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            // BƯỚC 2: Lấy ví của người dùng
            var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                return NotFound($"Wallet for user with ID {userId} not found.");
            }

            // BƯỚC 3: Lấy lịch sử giao dịch của ví và sắp xếp theo ngày gần nhất
            var transactions = await _unitOfWork.TransactionRepository.GetByWalletIdAsync(wallet.Id);
            var sortedTransactions = transactions.OrderByDescending(t => t.CreatedAt);

            // BƯỚC 4: Phân trang kết quả
            var paginatedTransactions = sortedTransactions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // BƯỚC 5: Chuyển đổi kết quả thành DTO
            var transactionDtos = paginatedTransactions.Select(t => new
            {
                t.Id,
                t.Amount,
                t.TransactionType,
                t.Status,
                t.CreatedAt,
                t.ReferenceId
            }).ToList();

            // BƯỚC 6: Tạo thông tin phân trang
            var totalItems = transactions.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // BƯỚC 7: Trả về kết quả
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
