using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        public async Task<IActionResult> PayForTrip(int tripId)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Trip not found.");

                var bookings = await _unitOfWork.BookingRepository.GetBookingsByTripIdAsync(tripId);
                if (bookings == null || !bookings.Any()) return NotFound("No bookings found for this trip.");

                var (totalCost, pricePerPerson, minPerson, maxPerson) = await _unitOfWork.TripTypePricingRepository.CalculateTripPrice(tripId);

                int actualParticipants = bookings.Count();
                int chargedParticipants = Math.Max(actualParticipants, minPerson);

                // Xử lý thanh toán
                foreach (var booking in bookings)
                {
                    var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(booking.UserId.Value);
                    if (wallet == null) return NotFound($"Wallet not found for user {booking.UserId}.");

                    if (wallet.Balance < pricePerPerson)
                    {
                        return BadRequest($"Insufficient balance for user {booking.UserId}.");
                    }

                    wallet.Balance -= pricePerPerson;
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                    var transaction = new Transaction
                    {
                        WalletId = wallet.Id,
                        Amount = pricePerPerson,
                        TransactionType = "Payment",
                        CreatedAt = DateTime.Now,
                        ReferenceId = $"Trip_{tripId}",
                        Status = 1
                    };
                    await _unitOfWork.TransactionRepository.CreateAsync(transaction);
                }

                trip.UnitPrice = pricePerPerson;
                trip.Status = 2; // Giả sử 2 là trạng thái "Đã thanh toán"
                await _unitOfWork.TripRepository.UpdateAsync(trip);

                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    message = "Payment successful",
                    totalAmountCharged = totalCost,
                    pricePerPerson,
                    actualParticipants,
                    chargedParticipants,
                    minPerson,
                    maxPerson,
                    potentialRefund = chargedParticipants > actualParticipants ? (chargedParticipants - actualParticipants) * pricePerPerson : 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during payment", error = ex.Message });
            }
        
    }
        [HttpPost("RefundTrip")]
        public async Task<IActionResult> RefundTrip(int tripId)
        {
            try
            {
                // Lấy thông tin chuyến đi
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Không tìm thấy chuyến đi.");

                // Lấy tất cả giao dịch liên quan đến chuyến đi từ bảng Transaction
                var transactions = await _unitOfWork.TransactionRepository
                    .GetTransactionsByTripIdAsync(tripId, "Payment");

                if (transactions == null || !transactions.Any())
                    return NotFound("Không tìm thấy giao dịch nào liên quan đến chuyến đi.");

                // Tính tổng số tiền đã thanh toán
                decimal totalAmountPaid = transactions.Sum(t => t.Amount ?? 0);

                // Lấy bảng giá của chuyến đi từ TripTypePricing
                var tripTypeId = trip.TripTypeId;
                var tripPricing = await _unitOfWork.TripTypePricingRepository
                    .GetPricingForTripAsync(tripTypeId, transactions.Count());

                if (tripPricing == null)
                {
                    return BadRequest("Không tìm thấy bảng giá của chuyến đi.");
                }

                // Tính tổng chi phí thực tế cho chuyến đi dựa trên giá MinPerson
                decimal totalActualCost = tripPricing.PricePerPerson * tripPricing.MinPerson;

                // Tính phần tiền dư để hoàn lại
                decimal totalRefundAmount = totalAmountPaid - totalActualCost;
                if (totalRefundAmount <= 0)
                    return BadRequest("Không cần hoàn tiền vì chi phí thực tế cao hơn hoặc bằng số tiền đã thu.");

                // Tính toán số tiền hoàn lại cho mỗi người tham gia
                decimal refundPerUser = totalRefundAmount / transactions.Count();

                // Tiến hành hoàn tiền cho từng người tham gia
                foreach (var transaction in transactions)
                {
                    var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(transaction.WalletId.Value);
                    if (wallet == null)
                    {
                        return NotFound($"Không tìm thấy ví cho người dùng {transaction.WalletId}");
                    }

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

                    // Lưu giao dịch hoàn tiền
                    await _unitOfWork.TransactionRepository.CreateAsync(refundTransaction);

                    // Cập nhật số dư của ví
                    wallet.Balance += refundPerUser;
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                }

                // Cập nhật trạng thái chuyến đi
                trip.Status = 3; // Hoàn tất
                await _unitOfWork.TripRepository.UpdateAsync(trip);

                // Lưu tất cả các thay đổi
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Hoàn tiền thành công cho tất cả người tham gia", refundPerUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình hoàn tiền", error = ex.Message });
            }
        }

    }
}