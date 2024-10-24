using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace STP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public TripController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var (pickUpLocation, dropOffLocation) = await GetLocations(request.PickUpLocationId, request.DropOffLocationId);

                if (pickUpLocation == null || dropOffLocation == null)
                {
                    return BadRequest("Invalid pickup or drop-off location.");
                }

                var tripType = await GetTripType(pickUpLocation.AreaId, dropOffLocation.AreaId);

                if (tripType == null)
                {
                    return BadRequest("Trip type not found for the provided locations.");
                }

                var trip = CreateTripEntity(request, pickUpLocation, dropOffLocation, tripType);

                await _unitOfWork.TripRepository.AddAsync(trip);

                return Ok(new { message = "Trip created successfully", tripId = trip.Id });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository.GetTripByIdWithLocationNamesAsync(id);

                if (trip == null)
                {
                    return NotFound($"Trip with ID {id} not found.");
                }

                return Ok(trip);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetTripList()
        {
            try
            {
                var trips = await _unitOfWork.TripRepository.GetAllTripsAsync();
                if (trips == null || trips.Count == 0)
                {
                    return NotFound("No trips available.");
                }

                return Ok(trips);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("availableTrips")]
        public async Task<IActionResult> GetAvailableTrips()
        {
            try
            {
                var trips = await _unitOfWork.TripRepository.GetAvailableTripsAsync();

                if (trips == null || trips.Count == 0)
                {
                    return NotFound("No available trips found.");
                }

                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //lich su chuyen di cua user
        [HttpGet("TripJoined/{userId}")]
        public async Task<IActionResult> GetTripsByUserId(int userId)
        {
            var trips = await _unitOfWork.TripRepository.GetTripsByUserIdAsync(userId);
            if (trips == null || !trips.Any())
            {
                return NotFound("No trips found for this user.");
            }
            return Ok(trips);
        }
        //thong ke
        [HttpGet("TripStatistics")]
        public async Task<IActionResult> GetTripStatistics()
        {
            var statistics = await _unitOfWork.TripRepository.GetTripStatisticsAsync();
            return Ok(statistics);
        }

        [HttpPatch("updateStatus/{id}")]
        public async Task<IActionResult> UpdateTripStatus(int id, [FromBody] UpdateTripStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(id);

                if (trip == null)
                {
                    return NotFound($"Trip with ID {id} not found.");
                }

                trip.Status = request.NewStatus;

                await _unitOfWork.TripRepository.UpdateAsync(trip);
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Trip status updated successfully", tripId = trip.Id, newStatus = trip.Status });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTripsAsync(
            [FromQuery] int? pickUpLocationId,
            [FromQuery] int? dropOffLocationId,
            [FromQuery] DateOnly? bookingDate,
            [FromQuery] TimeOnly? hourInDay,
            [FromQuery] int? availableSlots)
        {
            try
            {
                var trips = await _unitOfWork.TripRepository.SearchTripsAsync(
                    pickUpLocationId,
                    dropOffLocationId,
                    bookingDate,
                    hourInDay,
                    availableSlots);

                if (trips == null || trips.Count == 0)
                {
                    return NotFound("No trips found matching the criteria.");
                }

                return Ok(trips);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper methods remain unchanged
        private async Task<(Location pickUp, Location dropOff)> GetLocations(int pickUpId, int dropOffId)
        {
            var pickUpLocation = await _unitOfWork.LocationRepository.GetByIdAsync(pickUpId);
            var dropOffLocation = await _unitOfWork.LocationRepository.GetByIdAsync(dropOffId);

            return (pickUpLocation, dropOffLocation);
        }

        private async Task<TripType> GetTripType(int? pickUpAreaId, int? dropOffAreaId)
        {
            if (!pickUpAreaId.HasValue || !dropOffAreaId.HasValue)
            {
                throw new ArgumentException("Area ID is missing for pickup or drop-off location.");
            }

            return await _unitOfWork.TripTypeRepository.GetTripTypeByAreasAsync(pickUpAreaId.Value, dropOffAreaId.Value);
        }

        private Trip CreateTripEntity(CreateTripRequest request, Location pickUpLocation, Location dropOffLocation, TripType tripType)
        {
            return new Trip
            {
                PickUpLocationId = request.PickUpLocationId,
                DropOffLocationId = request.DropOffLocationId,
                ToAreaId = dropOffLocation.AreaId,
                MaxPerson = request.MaxPerson,
                MinPerson = request.MinPerson,
                UnitPrice = tripType.BasicePrice,
                BookingDate = request.BookingDate,
                HourInDay = request.HourInDay,
                TripTypeId = tripType.Id,
                Status = 1 // Assuming 1 means "Scheduled" or "Pending"
            };
        }
    }

    public class CreateTripRequest
    {
        public int PickUpLocationId { get; set; }
        public int DropOffLocationId { get; set; }
        public int MaxPerson { get; set; }
        public int MinPerson { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly HourInDay { get; set; }
    }

    public class UpdateTripStatusRequest
    {
        [Required]
        public int NewStatus { get; set; }
    }
}