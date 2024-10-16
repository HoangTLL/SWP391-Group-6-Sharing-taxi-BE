using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using STP.Repository;
using STP.Repository.Models;
using System.Threading.Tasks;
//ngày cập nhật 16/10/2024 - 3:18
namespace STP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        // Sử dụng UnitOfWork để quản lý các thao tác với database
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<WalletController> _logger;

        // Constructor để inject UnitOfWork và Logger vào controller
        public WalletController(UnitOfWork unitOfWork, ILogger<WalletController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // API nạp tiền vào ví
        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(int walletId, decimal amount, string method)
        {
            try
            {
                // Lấy ví từ database theo walletId
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
                if (wallet == null)
                {
                    // Trả về lỗi nếu ví không tồn tại
                    return NotFound("Wallet not found.");
                }

                // Tạo một bản ghi Deposit để lưu trữ giao dịch nạp tiền
                var deposit = new Deposit
                {
                    WalletId = walletId,
                    UserId = wallet.UserId ?? 0,  // Đảm bảo UserId không null
                    Amount = amount,
                    DepositMethod = method,
                    DepositDate = DateTime.Now,
                    Status = 1 // Trạng thái "Chờ xử lý"
                };

                // Lưu thông tin nạp tiền vào database
                await _unitOfWork.DepositRepository.CreateAsync(deposit);
                await _unitOfWork.SaveAsync(); // Lưu vào database để có Id của deposit

                // Sau khi tạo deposit, tạo transaction liên quan đến deposit vừa tạo
                var transaction = new Transaction
                {
                    WalletId = walletId,
                    Amount = amount,
                    TransactionType = "Deposit", // Loại giao dịch là "Nạp tiền"
                    CreatedAt = DateTime.Now,
                    ReferenceId = $"Deposit_{walletId}_{DateTime.Now.Ticks}", // Tạo mã tham chiếu
                    Status = 1,  // Đang xử lý
                    DepositId = deposit.Id // Gán DepositId từ deposit vừa tạo
                };

                // Lưu transaction vào database
                await _unitOfWork.TransactionRepository.CreateAsync(transaction);
                await _unitOfWork.SaveAsync(); // Lưu transaction để lấy Id

                // Cập nhật TransactionId trong Deposit sau khi transaction đã được tạo
                deposit.TransactionId = transaction.Id;
                await _unitOfWork.DepositRepository.UpdateAsync(deposit);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi

                // Cập nhật số dư của ví
                wallet.Balance += amount;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi vào database

                // Trả về thông báo thành công
                return Ok(new { message = "Deposit successful", balance = wallet.Balance });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi 500 nếu có ngoại lệ xảy ra
                return StatusCode(500, new { message = "An error occurred while processing your deposit.", error = ex.Message });
            }
        }

        // API để thanh toán cho chuyến đi
        [HttpPost("PayForTrip")]
        public async Task<IActionResult> PayForTrip(int tripId)
        {
            try
            {
                // Lấy thông tin chuyến đi từ cơ sở dữ liệu
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Trip not found.");

                // Lấy danh sách các đặt chỗ liên quan đến chuyến đi
                var bookings = await _unitOfWork.BookingRepository.GetBookingsByTripIdAsync(tripId);
                if (bookings == null || !bookings.Any()) return NotFound("No bookings found for this trip.");

                // Tính toán tổng chi phí và giá mỗi người dựa trên số lượng người tham gia
                var (totalCost, pricePerPerson, minPerson, maxPerson) = await _unitOfWork.TripTypePricingRepository.CalculateTripPrice(tripId);

                int actualParticipants = bookings.Count();
                int chargedParticipants = Math.Max(actualParticipants, minPerson); // Số người tối thiểu phải thanh toán

                // Xử lý thanh toán cho từng đặt chỗ
                foreach (var booking in bookings)
                {
                    var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(booking.UserId.Value);
                    if (wallet == null) return NotFound($"Wallet not found for user {booking.UserId}.");

                    // Kiểm tra số dư của người dùng
                    if (wallet.Balance < pricePerPerson)
                    {
                        return BadRequest($"Insufficient balance for user {booking.UserId}.");
                    }

                    // Trừ tiền từ ví của người dùng
                    wallet.Balance -= pricePerPerson;
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                    // Tạo transaction cho giao dịch thanh toán
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

                // Cập nhật thông tin chuyến đi sau khi thanh toán thành công
                trip.UnitPrice = pricePerPerson;
                trip.Status = 2; // Giả sử 2 là trạng thái "Đã thanh toán"
                await _unitOfWork.TripRepository.UpdateAsync(trip);

                await _unitOfWork.SaveAsync();

                // Trả về thông báo thành công và thông tin chi tiết về thanh toán
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
                // Xử lý ngoại lệ và trả về lỗi
                return StatusCode(500, new { message = "An error occurred during payment", error = ex.Message });
            }
        }
        [HttpGet("balance/{userId}")]
        public async Task<IActionResult> GetWalletBalance(int userId)
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
                // Trả về thông tin số dư
                return Ok(new
                {
                    UserId = userId,
                    Balance = wallet.Balance,
                    CurrencyCode = wallet.CurrencyCode
                });
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần
                return StatusCode(500, $"An error occurred while retrieving the wallet balance: {ex.Message}");
            }
        }

        // API để hoàn tiền cho chuyến đi
        [HttpPost("RefundTrip")]
        public async Task<IActionResult> RefundTrip(int tripId)
        {
            // Bắt đầu transaction để đảm bảo nếu có lỗi, tất cả các thay đổi sẽ được rollback
            using var dbTransaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"Processing refund for TripId: {tripId}");

                // Lấy thông tin chuyến đi từ cơ sở dữ liệu
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Không tìm thấy chuyến đi.");

                // Kiểm tra trạng thái của chuyến đi, nếu đã hoàn tất hoặc bị hủy thì không thể hoàn tiền
                if (trip.Status == 3 || trip.Status == 0)
                {
                    return BadRequest("Chuyến đi đã hoàn tất hoặc bị hủy, không thể hoàn tiền.");
                }

                // Kiểm tra nếu đã có giao dịch hoàn tiền trước đó
                var existingRefunds = await _unitOfWork.TransactionRepository.GetTransactionsByTripIdAsync(tripId, "Refund");
                if (existingRefunds.Any())
                {
                    return BadRequest("Chuyến đi này đã được hoàn tiền trước đó.");
                }

                // Lấy các giao dịch thanh toán liên quan đến chuyến đi
                var paymentTransactions = await _unitOfWork.TransactionRepository
                    .GetTransactionsByTripIdAsync(tripId, "Payment");

                if (!paymentTransactions.Any())
                    return NotFound("Không tìm thấy giao dịch thanh toán nào liên quan đến chuyến đi.");

                int actualParticipants = paymentTransactions.Count();

                // Tính toán lại giá trị thanh toán từ API thanh toán
                var (totalCost, pricePerPerson, minPerson, maxPerson) = await _unitOfWork.TripTypePricingRepository.CalculateTripPrice(tripId);

                // Lấy giá UnitPrice từ bảng Trip (nếu có)
                decimal unitPrice = trip.UnitPrice ?? 0; // UnitPrice lấy từ bảng Trip, nếu null thì mặc định là 0
                decimal totalActualCost = pricePerPerson * actualParticipants; // Tổng chi phí thực tế

                // Tính toán số tiền cần hoàn lại
                decimal totalRefundAmount = unitPrice - totalActualCost;
                if (totalRefundAmount <= 0)
                    return BadRequest("Không cần hoàn tiền vì chi phí thực tế cao hơn hoặc bằng số tiền đã thu.");

                // Tính tiền hoàn lại cho mỗi người
                decimal refundPerUser = Math.Round(totalRefundAmount / actualParticipants, 2);

                // Tiến hành hoàn tiền cho từng người tham gia
                foreach (var paymentTransaction in paymentTransactions)
                {
                    if (!paymentTransaction.WalletId.HasValue)
                    {
                        return BadRequest($"WalletId is null for transaction {paymentTransaction.Id}");
                    }

                    var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(paymentTransaction.WalletId.Value);
                    if (wallet == null)
                    {
                        return NotFound($"Không tìm thấy ví cho người dùng {paymentTransaction.WalletId}");
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

                    await _unitOfWork.TransactionRepository.CreateAsync(refundTransaction);

                    // Cập nhật số dư ví
                    wallet.Balance += refundPerUser;
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                    // Tạo bản ghi trong bảng Deposit để theo dõi
                    var deposit = new Deposit
                    {
                        WalletId = wallet.Id,
                        UserId = wallet.UserId ?? 0,
                        Amount = refundPerUser,
                        DepositMethod = "Refund",
                        DepositDate = DateTime.Now,
                        Status = 1, // Đã xử lý
                        TransactionId = refundTransaction.Id
                    };

                    await _unitOfWork.DepositRepository.CreateAsync(deposit);

                    // Cập nhật TransactionId trong bảng Transaction
                    refundTransaction.DepositId = deposit.Id;
                    await _unitOfWork.TransactionRepository.UpdateAsync(refundTransaction);
                }

                // Cập nhật trạng thái chuyến đi sau khi hoàn tiền
                trip.Status = 3;  // Hoàn tất
                await _unitOfWork.TripRepository.UpdateAsync(trip);

                await _unitOfWork.SaveAsync();
                await dbTransaction.CommitAsync();

                return Ok(new { message = "Hoàn tiền thành công cho tất cả người tham gia", refundPerUser });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và rollback nếu có ngoại lệ xảy ra
                await dbTransaction.RollbackAsync();
                _logger.LogError(ex, $"Error processing refund for TripId: {tripId}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình hoàn tiền", error = ex.Message });
            }
        }
    }
}
