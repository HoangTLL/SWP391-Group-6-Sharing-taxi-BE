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
        public async Task<IActionResult> Deposit(int userId, decimal amount, string method)
        {
            try
            {
                // Lấy người dùng từ database theo userId
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    // Trả về lỗi nếu người dùng không tồn tại
                    return NotFound("User not found.");
                }

                // Lấy ví của người dùng dựa vào userId
                var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(userId);
                if (wallet == null)
                {
                    // Trả về lỗi nếu ví không tồn tại
                    return NotFound("Wallet not found for this user.");
                }

                // Tạo một bản ghi Deposit để lưu trữ giao dịch nạp tiền
                var deposit = new Deposit
                {
                    WalletId = wallet.Id,
                    UserId = userId,  // Sử dụng userId từ tham số
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
                    WalletId = wallet.Id,
                    Amount = amount,
                    TransactionType = "Deposit", // Loại giao dịch là "Nạp tiền"
                    CreatedAt = DateTime.Now,
                    ReferenceId = $"Deposit_{wallet.Id}_{DateTime.Now.Ticks}", // Tạo mã tham chiếu
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

        /// <summary>
        /// API để xử lý thanh toán cho một chuyến đi
        /// Cho phép thanh toán đồng thời cho nhiều người trong cùng một chuyến
        /// </summary>
        /// <param name = "tripId" > ID của chuyến đi cần thanh toán</param>
        /// <returns>Kết quả thanh toán kèm thông tin chi tiết</returns>
        [HttpPost("PayForTrip")]
        public async Task<IActionResult> PayForTrip(int tripId)
        {
            try
            {
                // BƯỚC 1: KIỂM TRA VÀ LẤY THÔNG TIN CHUYẾN ĐI
                // 1.1: Truy vấn thông tin chuyến đi từ database theo tripId
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                // Nếu không tìm thấy chuyến đi, trả về lỗi 404
                if (trip == null) return NotFound("Trip not found.");

                // 1.2: Lấy tất cả các booking (đặt chỗ) của chuyến đi này
                var bookings = await _unitOfWork.BookingRepository.GetBookingsByTripIdAsync(tripId);
                // Kiểm tra nếu không có booking nào hoặc danh sách rỗng
                if (bookings == null || !bookings.Any()) return NotFound("No bookings found for this trip.");

                // BƯỚC 2: TÍNH TOÁN CHI PHÍ VÀ SỐ NGƯỜI
                // 2.1: Lấy thông tin giá và số lượng người từ bảng TripTypePricing
                var (totalCost,                // Tổng chi phí chuyến đi
                    pricePerPerson,            // Giá tiền trên mỗi người
                    minPerson,                 // Số người tối thiểu
                    maxPerson                  // Số người tối đa
                    ) = await _unitOfWork.TripTypePricingRepository.CalculateTripPrice(tripId);

                // 2.2: Tính toán số người thực tế và số người cần tính phí
                int actualParticipants = bookings.Count();  // Số người thực tế đăng ký
                                                            // Số người tính phí sẽ là max giữa số người thực tế và số người tối thiểu
                int chargedParticipants = Math.Max(actualParticipants, minPerson);

                // BƯỚC 3: LỌC CÁC BOOKING CẦN THANH TOÁN
                // 3.1: Chỉ lấy những booking có status = 1 (đang chờ thanh toán)
                var pendingBookings = bookings.Where(b => b.Status == 1).ToList();

                // 3.2: Kiểm tra nếu không có booking nào cần thanh toán
                if (!pendingBookings.Any())
                {
                    return BadRequest("No pending payments found for this trip.");
                }

                // BƯỚC 4: KHỞI TẠO DANH SÁCH THEO DÕI KẾT QUẢ
                // 4.1: Danh sách lưu các booking đã xử lý thanh toán thành công
                var processedBookings = new List<Booking>();
                // 4.2: Danh sách lưu các booking thất bại và lý do
                var failedBookings = new List<(Booking booking, string reason)>();

                // BƯỚC 5: XỬ LÝ THANH TOÁN TỪNG BOOKING
                foreach (var booking in pendingBookings)
                {
                    try
                    {
                        // 5.1: Kiểm tra UserId có tồn tại
                        if (!booking.UserId.HasValue)
                        {
                            // Nếu không có UserId, thêm vào danh sách thất bại
                            failedBookings.Add((booking, "User ID is missing"));
                            continue; // Bỏ qua booking này, xử lý booking tiếp theo
                        }

                        // 5.2: Lấy thông tin ví của user
                        var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(booking.UserId.Value);
                        if (wallet == null)
                        {
                            // Nếu không tìm thấy ví, thêm vào danh sách thất bại
                            failedBookings.Add((booking, "Wallet not found"));
                            continue;
                        }

                        // 5.3: Kiểm tra số dư trong ví
                        if (wallet.Balance < pricePerPerson)
                        {
                            // Nếu số dư không đủ, thêm vào danh sách thất bại
                            failedBookings.Add((booking, "Insufficient balance"));
                            continue;
                        }

                        // 5.4: Trừ tiền từ ví
                        wallet.Balance -= pricePerPerson;
                        await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                        // 5.5: Tạo transaction ghi nhận giao dịch thanh toán
                        var transaction = new Transaction
                        {
                            WalletId = wallet.Id,           // ID của ví
                            Amount = pricePerPerson,        // Số tiền thanh toán
                            TransactionType = "Payment",    // Loại giao dịch: Thanh toán
                            CreatedAt = DateTime.Now,       // Thời gian tạo giao dịch
                            ReferenceId = $"Trip_{tripId}", // Mã tham chiếu đến chuyến đi
                            Status = 1                      // Trạng thái: Thành công
                        };
                        await _unitOfWork.TransactionRepository.CreateAsync(transaction);

                        // 5.6: Cập nhật trạng thái booking sang đã thanh toán (status = 2)
                        booking.Status = 2;  // 2 = Đã thanh toán
                        await _unitOfWork.BookingRepository.UpdateAsync(booking);

                        // 5.7: Thêm booking vào danh sách xử lý thành công
                        processedBookings.Add(booking);
                    }
                    catch (Exception ex)
                    {
                        // 5.8: Xử lý lỗi cho từng booking riêng biệt
                        failedBookings.Add((booking, ex.Message));
                        // Ghi log lỗi
                        _logger.LogError($"Error processing payment for booking {booking.Id}: {ex.Message}");
                    }
                }

                // BƯỚC 6: CẬP NHẬT THÔNG TIN CHUYẾN ĐI
                if (processedBookings.Any())
                {
                    // 6.1: Cập nhật giá trên mỗi người vào thông tin chuyến đi
                    trip.UnitPrice = pricePerPerson;
                    await _unitOfWork.TripRepository.UpdateAsync(trip);
                    // 6.2: Lưu tất cả thay đổi vào database
                    await _unitOfWork.SaveAsync();
                }

                // BƯỚC 7: CHUẨN BỊ THÔNG TIN PHẢN HỒI
                var response = new
                {
                    // 7.1: Thông báo kết quả chung
                    message = processedBookings.Any() ? "Payments processed" : "No payments were processed",

                    // 7.2: Danh sách các thanh toán thành công
                    successfulPayments = processedBookings.Select(b => new
                    {
                        bookingId = b.Id,
                        userId = b.UserId,
                        amount = pricePerPerson
                    }).ToList(),

                    // 7.3: Danh sách các thanh toán thất bại
                    failedPayments = failedBookings.Select(f => new
                    {
                        bookingId = f.booking.Id,
                        userId = f.booking.UserId,
                        reason = f.reason
                    }).ToList(),

                    // 7.4: Thông tin tổng kết
                    summary = new
                    {
                        totalProcessed = processedBookings.Count,    // Số lượng xử lý thành công
                        totalFailed = failedBookings.Count,         // Số lượng xử lý thất bại
                        pricePerPerson,                            // Giá trên mỗi người
                        actualParticipants,                        // Số người thực tế
                        chargedParticipants,                       // Số người tính phí
                        minPerson,                                 // Số người tối thiểu
                        maxPerson,                                 // Số người tối đa
                                                                   // Số tiền có thể hoàn lại nếu số người tính phí > số người thực tế
                        potentialRefund = chargedParticipants > actualParticipants ?
                            (chargedParticipants - actualParticipants) * pricePerPerson : 0
                    }
                };

                // BƯỚC 8: TRẢ VỀ KẾT QUẢ VỚI STATUS CODE PHÙ HỢP
                // 8.1: Nếu có cả thanh toán thành công và thất bại
                if (processedBookings.Any() && failedBookings.Any())
                {
                    return StatusCode(207, response); // 207 Multi-Status
                }
                // 8.2: Nếu tất cả đều thất bại
                else if (failedBookings.Any() && !processedBookings.Any())
                {
                    return BadRequest(response); // 400 Bad Request
                }
                // 8.3: Nếu tất cả đều thành công
                else
                {
                    return Ok(response); // 200 OK
                }
            }
            catch (Exception ex)
            {
                // BƯỚC 9: XỬ LÝ LỖI TỔNG THỂ CỦA API
                // Ghi log lỗi và trả về lỗi 500 Internal Server Error
                return StatusCode(500, new
                {
                    message = "An error occurred during payment",
                    error = ex.Message
                });
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
        [HttpPost("RefundOnLeaveTrip")]
        public async Task<IActionResult> RefundOnLeaveTrip(int userId, int tripId)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"Processing refund for User {userId} leaving Trip {tripId}");

                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found.");

                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Trip not found.");

                var booking = await _unitOfWork.BookingRepository.GetBookingByUserAndTripAsync(userId, tripId);
                if (booking == null) return NotFound("Booking not found for this user and trip.");

                if (trip.Status != 1) // Assuming 1 is "Pending" status
                {
                    return BadRequest("Cannot refund. Trip is not in a pending state.");
                }

                var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(userId);
                if (wallet == null) return NotFound("Wallet not found for this user.");

                // Calculate refund amount (you might want to adjust this based on your business logic)
                decimal refundAmount = trip.UnitPrice ?? 0;

                // Create refund transaction
                var refundTransaction = new Transaction
                {
                    WalletId = wallet.Id,
                    Amount = refundAmount,
                    TransactionType = "Refund",
                    CreatedAt = DateTime.UtcNow,
                    ReferenceId = $"Refund_LeaveTrip_{tripId}_{userId}",
                    Status = 1 // Assuming 1 is "Processed" status
                };

                await _unitOfWork.TransactionRepository.CreateAsync(refundTransaction);

                // Update wallet balance
                wallet.Balance += refundAmount;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                // Update booking status
                booking.Status = 0; // Assuming 0 is "Cancelled" status
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Refund processed successfully",
                    refundAmount = refundAmount,
                    newBalance = wallet.Balance
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"Error processing refund for User {userId} leaving Trip {tripId}");
                return StatusCode(500, new { message = "An error occurred during the refund process", error = ex.Message });
            }
        }
    }
}
