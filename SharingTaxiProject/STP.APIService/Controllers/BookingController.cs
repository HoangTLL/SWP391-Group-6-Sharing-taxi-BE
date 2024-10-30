using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
using System;
using System.Threading.Tasks;

namespace STP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// API to check if a trip is full based on the trip ID.
        /// </summary>
        /// <param name="tripId">ID of the trip to check.</param>
        /// <returns>Whether the trip is full and current bookings count.</returns>
        [HttpGet("checkTripFull/{tripId}")]
        public async Task<IActionResult> CheckTripFull(int tripId)
        {
            try
            {
                // BƯỚC 1: Truy vấn thông tin chuyến đi từ database
                var trip = await _unitOfWork.TripRepository.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    return NotFound("Trip not found.");
                }

                // BƯỚC 2: Đếm số lượng booking hiện tại của chuyến đi
                var currentBookingsCount = await _unitOfWork.BookingRepository.CountBookingsByTripIdAsync(tripId);

                // BƯỚC 3: Kiểm tra chuyến đi đã đầy chưa
                bool isFull = currentBookingsCount >= trip.MaxPerson;
                return Ok(new { isFull, currentBookingsCount, maxPerson = trip.MaxPerson });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// API to allow a user to join a trip.
        /// </summary>
        /// <param name="request">Details of the user and trip to join.</param>
        /// <returns>Success or error message upon joining the trip.</returns>
        [HttpPost("joinTrip")]
        public async Task<IActionResult> JoinTrip([FromBody] JoinTripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // BƯỚC 1: Kiểm tra nếu người dùng đã tham gia chuyến đi này
                var existingBooking = await _unitOfWork.BookingRepository.GetBookingByUserAndTripAsync(request.UserId, request.TripId);
                if (existingBooking != null)
                {
                    return BadRequest("User has already joined this trip.");
                }

                // BƯỚC 2: Tạo một booking mới cho chuyến đi
                var booking = new Booking
                {
                    UserId = request.UserId,
                    TripId = request.TripId,
                    Status = 1
                };

                // BƯỚC 3: Lưu booking vào database
                var result = await _unitOfWork.BookingRepository.CreateBookingAsync(booking);
                if (result)
                {
                    return Ok("Successfully joined the trip.");
                }

                return StatusCode(500, "An error occurred while joining the trip.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// API to retrieve the list of users in a specified trip.
        /// </summary>
        /// <param name="tripId">ID of the trip to retrieve users for.</param>
        /// <returns>List of users in the trip.</returns>
        [HttpGet("usersInTrip/{tripId}")]
        public async Task<IActionResult> GetUsersInTrip(int tripId)
        {
            try
            {
                // BƯỚC 1: Truy vấn thông tin chuyến đi từ database
                var trip = await _unitOfWork.TripRepository.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    return NotFound("Trip not found.");
                }

                // BƯỚC 2: Lấy danh sách tất cả booking cho chuyến đi
                var bookings = await _unitOfWork.BookingRepository.GetBookingsByTripIdAsync(tripId);

                // BƯỚC 3: Kiểm tra nếu không có booking nào trong chuyến đi
                if (bookings == null || !bookings.Any())
                {
                    return Ok(new { message = "No users have joined this trip." });
                }

                // BƯỚC 4: Lấy danh sách tên người dùng từ booking
                var usersInTrip = bookings.Select(b => new { b.User.Id, b.User.Name }).ToList();

                return Ok(usersInTrip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// API to allow a user to leave a trip.
        /// </summary>
        /// <param name="request">Details of the user and trip to leave.</param>
        /// <returns>Success or error message upon leaving the trip.</returns>
        [HttpDelete("outTrip")]
        public async Task<IActionResult> OutTrip([FromBody] OutTripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // BƯỚC 1: Kiểm tra nếu booking của người dùng cho chuyến đi tồn tại
                var booking = await _unitOfWork.BookingRepository.GetBookingByUserAndTripAsync(request.UserId, request.TripId);
                if (booking == null)
                {
                    return NotFound("Booking not found.");
                }

                // BƯỚC 2: Xóa booking khỏi database
                var result = await _unitOfWork.BookingRepository.DeleteBookingAsync(booking);
                if (result)
                {
                    return Ok(new { message = "Successfully left the trip" });
                }
                else
                {
                    return StatusCode(500, "Failed to delete booking.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và ghi log
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class JoinTripRequest
    {
        public int UserId { get; set; }
        public int TripId { get; set; }
    }

    public class OutTripRequest
    {
        public int UserId { get; set; }
        public int TripId { get; set; }
    }
}
