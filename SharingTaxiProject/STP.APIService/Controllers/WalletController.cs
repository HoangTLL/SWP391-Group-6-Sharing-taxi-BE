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
                // Lấy thông tin chuyến đi
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Trip not found.");

                // Lấy danh sách các booking của chuyến đi
                var bookings = await _unitOfWork.BookingRepository.GetBookingsByTripIdAsync(tripId);
                if (bookings == null || !bookings.Any()) return NotFound("No bookings found for this trip.");

                // Tính giá cho chuyến đi dựa trên số người tham gia (số lượng booking)
                decimal totalPricePerPerson = await _unitOfWork.TripRepository.CalculateTripPrice(trip.PickUpLocationId.Value, trip.DropOffLocationId.Value, bookings.Count());

                // Duyệt qua danh sách người tham gia để trừ tiền
                foreach (var booking in bookings)
                {
                    // Lấy thông tin ví của từng người dùng trong bảng Booking
                    var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(booking.UserId.Value);
                    if (wallet == null) return NotFound($"Wallet not found for user {booking.UserId}.");

                    // Kiểm tra số dư của ví
                    if (wallet.Balance < totalPricePerPerson)
                    {
                        return BadRequest($"Insufficient balance for user {booking.UserId}.");
                    }

                    // Trừ tiền từ ví của từng người
                    wallet.Balance -= totalPricePerPerson;

                    // Cập nhật thông tin ví
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                }

                // Lưu các thay đổi
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Payment successful", totalAmountCharged = totalPricePerPerson * bookings.Count() });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during payment", error = ex.Message });
            }
        }

        [HttpPost("RefundTrip")]
        public async Task<IActionResult> RefundTrip(int tripId , int TripType)
        {
            try
            {
                // 1. Lấy thông tin chuyến đi
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripId);
                if (trip == null) return NotFound("Không tìm thấy chuyến đi.");

                // 2. Lấy danh sách những người tham gia chuyến đi
                var participants = await _unitOfWork.TripRepository.GetTripParticipantsAsync(tripId);
                if (participants == null || !participants.Any())
                    return NotFound("Không tìm thấy người tham gia cho chuyến đi này.");

                // 3. Tạo DTO để chứa thông tin cần thiết về người tham gia
                var participantDtos = participants.Select(p => new
                {
                    UserId = p.UserId,
                    AmountPaid = p.AmountPaid
                }).ToList();

                // 4. Tính tổng số tiền mà người tham gia đã trả
                decimal totalAmountCharged = participantDtos.Sum(p => p.AmountPaid);

                // 5. Tính chi phí thực tế dựa trên số người tham gia và giá vé
                var tripPricings = await _unitOfWork.TripTypePricingRepository.GetPricingByTripTypeAsync(TripType);
                if (tripPricings == null || !tripPricings.Any())
                    return BadRequest("Không tìm thấy bảng giá của chuyến đi.");

                // Lấy phần tử đầu tiên trong danh sách (nếu có nhiều phần tử)
                var tripPricing = tripPricings.FirstOrDefault();
                if (tripPricing == null)
                    return BadRequest("Không tìm thấy bảng giá của chuyến đi.");

                decimal totalActualCost = participantDtos.Count * tripPricing.PricePerPerson;

                // 6. Kiểm tra xem có cần hoàn tiền không
                decimal totalRefundAmount = totalAmountCharged - totalActualCost;
                if (totalRefundAmount <= 0)
                    return BadRequest("Không cần hoàn tiền vì chi phí thực tế cao hơn hoặc bằng số tiền đã thu.");

                // 7. Tính toán số tiền hoàn lại cho mỗi người tham gia
                decimal refundPerUser = totalRefundAmount / participantDtos.Count;

                // 8. Tiến hành hoàn tiền cho từng người tham gia
                foreach (var participantDto in participantDtos)
                {
                    var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAsync(participantDto.UserId);
                    if (wallet == null) return NotFound($"Không tìm thấy ví cho người dùng {participantDto.UserId}");

                    // Tạo giao dịch hoàn tiền
                    var refundTransaction = new Transaction
                    {
                        WalletId = wallet.Id,
                        Amount = refundPerUser,
                        TransactionType = "Hoàn tiền",
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

                // 9. Lưu tất cả các thay đổi vào cơ sở dữ liệu
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