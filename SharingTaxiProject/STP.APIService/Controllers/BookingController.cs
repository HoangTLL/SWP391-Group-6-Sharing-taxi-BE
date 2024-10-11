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
                // Check if the user is already booked for this trip
                var existingBooking = await _unitOfWork.BookingRepository.GetBookingByUserAndTripAsync(request.UserId, request.TripId);
                if (existingBooking != null)
                {
                    return BadRequest("User has already joined this trip.");
                }

                // Retrieve trip details to check the maximum number of passengers
                var trip = await _unitOfWork.TripRepository.GetTripByIdAsync(request.TripId);
                if (trip == null)
                {
                    return NotFound("Trip not found.");
                }

                // Count the number of current bookings for the trip
                var currentBookingsCount = await _unitOfWork.BookingRepository.CountBookingsByTripIdAsync(request.TripId);

                // Check if the trip is already full
                if (currentBookingsCount >= trip.MaxPerson)
                {
                    return BadRequest("Trip is full. Cannot join.");
                }

                // Create a new booking if the trip is not full
                var booking = new Booking
                {
                    UserId = request.UserId,
                    TripId = request.TripId,
                    Status = 1
                };

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