using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
using System;
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
                PricingId = tripType.Id,
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
        public DateTime BookingDate { get; set; }
        public TimeOnly HourInDay { get; set; }
    }
}