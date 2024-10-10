using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public WalletController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(int walletId, decimal amount, string method)
        {
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null)
                {
                    return NotFound("Wallet not found.");
                }

                // Tạo Deposit trước
                var deposit = new Deposit
                {
                    WalletId = walletId,
                    UserId = wallet.UserId ?? 0,  // Đảm bảo UserId không null
                    Amount = amount,
                    DepositMethod = method,
                    DepositDate = DateTime.Now,
                    Status = 1 // Chờ xử lý
                };

                await _unitOfWork.DepositRepository.CreateAsync(deposit);
                await _unitOfWork.SaveAsync(); // Lưu vào database để có Id của deposit

                // Sau khi tạo deposit, tạo transaction liên quan đến deposit vừa tạo
                var transaction = new Transaction
                {
                    WalletId = walletId,
                    Amount = amount,
                    TransactionType = "Deposit",
                    CreatedAt = DateTime.Now,
                    ReferenceId = $"Deposit_{walletId}_{DateTime.Now.Ticks}",
                    Status = 1,  // Đang xử lý
                    DepositId = deposit.Id // Gán DepositId từ deposit vừa tạo
                };

                await _unitOfWork.TransactionRepository.CreateAsync(transaction);
                await _unitOfWork.SaveAsync(); // Lưu transaction để lấy Id

                // Cập nhật lại TransactionId trong Deposit sau khi transaction đã được tạo
                deposit.TransactionId = transaction.Id;
                await _unitOfWork.DepositRepository.UpdateAsync(deposit);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi

                // Cập nhật số dư của ví
                wallet.Balance += amount;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi vào database

                return Ok(new { message = "Deposit successful", balance = wallet.Balance });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về lỗi
                return StatusCode(500, new { message = "An error occurred while processing your deposit.", error = ex.Message });
            }
        }


        [HttpPost("PayForTrip")]
        public async Task<IActionResult> PayForTrip(int walletId, int tripId, decimal amount)
        {
            try
            {
               
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null) return NotFound("Wallet not found.");
                if (wallet.Balance < amount) return BadRequest("Insufficient balance.");

                var transaction = new Transaction
                {
                    WalletId = walletId,
                    
                    Amount = -amount,
                    TransactionType = "Payment",
                    CreatedAt = DateTime.Now,
                    ReferenceId = $"Trip_{tripId}",
                    Status = 1 // Đang xử lý
                };

                await _unitOfWork.TransactionRepository.CreateAsync(transaction);

                wallet.Balance -= amount;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Payment successful", balance = wallet.Balance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during payment", error = ex.Message });
            }
        }

        [HttpPost("Refund")]
        public async Task<IActionResult> Refund(int walletId, int tripId, decimal amount)
        {
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null) return NotFound("Wallet not found.");

                var refund = new Transaction
                {
                    WalletId = walletId,
                    Amount = amount,
                    TransactionType = "Refund",
                    CreatedAt = DateTime.Now,
                    ReferenceId = $"Refund_Trip_{tripId}",
                    Status = 1 // Đang xử lý
                };

                await _unitOfWork.TransactionRepository.CreateAsync(refund);

                wallet.Balance += amount;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Refund successful", balance = wallet.Balance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during refund", error = ex.Message });
            }
        }

        [HttpGet("Balance")]
        public async Task<IActionResult> CheckBalance(int walletId)
        {
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null) return NotFound("Wallet not found.");

                return Ok(new { balance = wallet.Balance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking balance", error = ex.Message });
            }
        }
        [HttpGet("Transactions")]
        public async Task<IActionResult> GetTransactions(int walletId)
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetByWalletIdAsync(walletId);
                if (transactions == null || !transactions.Any()) return NotFound("No transactions found.");

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching transactions", error = ex.Message });
            }
        }

    }
}