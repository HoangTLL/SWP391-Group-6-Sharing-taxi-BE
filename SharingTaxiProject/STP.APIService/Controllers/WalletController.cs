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
        public async Task<IActionResult> PayForTrip(int walletId, int tripId, int numberOfParticipants)
        {
            try
            {
                // Lấy thông tin chuyến đi
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Trip not found.");

                // Tính giá cho chuyến đi dựa trên số người tham gia
                decimal totalPrice = await CalculateTripPrice(trip.TripTypeId, numberOfParticipants);

                // Lấy thông tin ví và kiểm tra số dư
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null) return NotFound("Wallet not found.");
                if (wallet.Balance < totalPrice) return BadRequest("Insufficient balance.");

                // Tạo giao dịch thanh toán
                var transaction = new Transaction
                {
                    WalletId = walletId,
                    Amount = -totalPrice,
                    TransactionType = "Payment",
                    CreatedAt = DateTime.Now,
                    ReferenceId = $"Trip_{tripId}",
                    Status = 1 // Đang xử lý
                };

                // Ghi giao dịch vào cơ sở dữ liệu
                await _unitOfWork.TransactionRepository.CreateAsync(transaction);

                // Cập nhật số dư ví
                wallet.Balance -= totalPrice;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Payment successful", totalAmount = totalPrice, balance = wallet.Balance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during payment", error = ex.Message });
            }
        }

        [HttpPost("RefundTrip")]
        public async Task<IActionResult> RefundTrip(int tripId, decimal actualTripCost)
        {
            try
            {
                // Lấy thông tin chuyến đi
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Trip not found.");

                // Lấy danh sách tất cả người dùng tham gia chuyến đi
                var participants = await _unitOfWork.TripRepository.GetTripParticipantsAsync(tripId);
                if (participants == null || !participants.Any())
                    return NotFound("No participants found for the trip.");

                // Tính tổng số tiền mà tất cả người dùng đã trả
                decimal totalAmountCharged = participants.Sum(p => p.AmountPaid);

                // Tính số tiền dư cần hoàn trả
                decimal totalRefundAmount = totalAmountCharged - actualTripCost;
                if (totalRefundAmount <= 0)
                    return BadRequest("No refund is necessary as the actual trip cost is higher than or equal to the charged amount.");

                // Chia đều số tiền dư cho tất cả người dùng
                decimal refundPerUser = totalRefundAmount / participants.Count;

                // Hoàn tiền cho từng người dùng
                foreach (var participant in participants)
                {
                    var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(participant.UserId);
                    if (wallet == null) return NotFound($"Wallet not found for user {participant.UserId}");

                    // Tạo giao dịch hoàn tiền
                    var refundTransaction = new Transaction
                    {
                        WalletId = wallet.Id,
                        Amount = refundPerUser,
                        TransactionType = "Refund",
                        CreatedAt = DateTime.Now,
                        ReferenceId = $"Refund_Trip_{tripId}",
                        Status = 1 // Đang xử lý
                    };

                    // Ghi giao dịch vào cơ sở dữ liệu
                    await _unitOfWork.TransactionRepository.CreateAsync(refundTransaction);

                    // Cập nhật số dư của người dùng
                    wallet.Balance += refundPerUser;
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                }

                // Lưu các thay đổi
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Refund successful for all participants", refundPerUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during the refund process", error = ex.Message });
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