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

        [HttpGet("checkTripFull/{tripId}")]
        public async Task<IActionResult> CheckTripFull(int tripId)
        {
            try
            {
                // Retrieve trip details
                var trip = await _unitOfWork.TripRepository.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    return NotFound("Trip not found.");
                }

                // Count the number of current bookings for the trip
                var currentBookingsCount = await _unitOfWork.BookingRepository.CountBookingsByTripIdAsync(tripId);

                // Check if the trip is full
                bool isFull = currentBookingsCount >= trip.MaxPerson;
                return Ok(new { isFull, currentBookingsCount, maxPerson = trip.MaxPerson });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

                // Create a new booking
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
        [HttpGet("usersInTrip/{tripId}")]
        public async Task<IActionResult> GetUsersInTrip(int tripId)
        {
            try
            {
                // Retrieve the trip by ID
                var trip = await _unitOfWork.TripRepository.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    return NotFound("Trip not found.");
                }

                // Retrieve all bookings for the trip
                var bookings = await _unitOfWork.BookingRepository.GetBookingsByTripIdAsync(tripId);

                // Check if there are bookings for the trip
                if (bookings == null || !bookings.Any())
                {
                    return Ok(new { message = "No users have joined this trip." });
                }

                // Retrieve the list of user names from the bookings
                var usersInTrip = bookings.Select(b => new { b.User.Id, b.User.Name }).ToList();

                return Ok(usersInTrip);
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