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

        [HttpPost("joinTrip")]
        public async Task<IActionResult> JoinTrip([FromBody] JoinTripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingBooking = await _unitOfWork.BookingRepository.GetBookingByUserAndTripAsync(request.UserId, request.TripId);
                if (existingBooking != null)
                {
                    return BadRequest("User has already joined this trip.");
                }

                var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);
                if (trip == null)
                {
                    return NotFound("Trip not found.");
                }

                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var newBooking = new Booking
                {
                    UserId = request.UserId,
                    TripId = request.TripId,
                    Status = 1 // Assuming 1 means "Active" or "Confirmed"
                };

                var result = await _unitOfWork.BookingRepository.CreateBookingAsync(newBooking);
                if (result)
                {
                    return Ok(new { message = "Successfully joined the trip", bookingId = newBooking.Id });
                }
                else
                {
                    return StatusCode(500, "Failed to create booking.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("outTrip")]
        public async Task<IActionResult> OutTrip([FromBody] OutTripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var booking = await _unitOfWork.BookingRepository.GetBookingByUserAndTripAsync(request.UserId, request.TripId);
                if (booking == null)
                {
                    return NotFound("Booking not found.");
                }

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
                // Log the exception
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