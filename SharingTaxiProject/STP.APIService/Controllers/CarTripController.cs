using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using STP.Repository;
using STP.Repository.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace STP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarTripController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<CarTripController> _logger;

        public CarTripController(UnitOfWork unitOfWork, ILogger<CarTripController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/CarTrip
        [HttpGet]
        public async Task<IActionResult> GetCarTrips()
        {
            _logger.LogInformation("Getting all car trips.");

            var carTrips = (await _unitOfWork.CarTripRepository.GetAllAsync())
                .Select(ct => new CarTripDto
                {
                    TripId = ct.TripId,
                    DriverName = ct.DriverName,
                    DriverPhone = ct.DriverPhone,
                    PlateNumber = ct.PlateNumber,
                    ArrivedTime = ct.ArrivedTime,
                    Status = ct.Status
                })
                .ToList();

            return Ok(carTrips);
        }

        // GET: api/CarTrip/{tripId}
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetCarTrip(int? tripId)
        {
            if (!tripId.HasValue)
            {
                return BadRequest("TripId must be provided.");
            }

            _logger.LogInformation($"Getting car trip with TripId: {tripId}");

            var carTrip = await _unitOfWork.CarTripRepository.GetByTripIdAsync(tripId.Value);
            if (carTrip == null)
            {
                _logger.LogWarning($"Car trip with TripId {tripId} not found.");
                return NotFound("Car trip not found.");
            }

            var carTripDto = new CarTripDto
            {
                TripId = carTrip.TripId,
                DriverName = carTrip.DriverName,
                DriverPhone = carTrip.DriverPhone,
                PlateNumber = carTrip.PlateNumber,
                ArrivedTime = carTrip.ArrivedTime,
                Status = carTrip.Status
            };

            return Ok(carTripDto);
        }

        // POST: api/CarTrip
        [HttpPost]
        public async Task<IActionResult> CreateCarTrip([FromBody] CarTripCreateDto carTripDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating a car trip.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Creating new car trip for TripId: {carTripDto.TripId}");

                var carTrip = new CarTrip
                {
                    TripId = carTripDto.TripId,
                    DriverName = carTripDto.DriverName,
                    DriverPhone = carTripDto.DriverPhone,
                    PlateNumber = carTripDto.PlateNumber,
                    ArrivedTime = carTripDto.ArrivedTime,
                    Status = carTripDto.Status
                };

                await _unitOfWork.CarTripRepository.AddAsync(carTrip);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(GetCarTrip), new { tripId = carTrip.TripId }, carTrip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a car trip.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/CarTrip/{tripId}
        [HttpPut("{tripId}")]
        public async Task<IActionResult> UpdateCarTrip(int? tripId, [FromBody] CarTripUpdateDto carTripDto)
        {
            if (!tripId.HasValue)
            {
                return BadRequest("TripId must be provided.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid model state during update for car trip TripId {tripId}.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Updating car trip with TripId: {tripId}");
                var existingCarTrip = await _unitOfWork.CarTripRepository.GetByTripIdAsync(tripId.Value);
                if (existingCarTrip == null)
                {
                    _logger.LogWarning($"Car trip with TripId {tripId} not found during update.");
                    return NotFound("Car trip not found.");
                }

                existingCarTrip.DriverName = carTripDto.DriverName;
                existingCarTrip.DriverPhone = carTripDto.DriverPhone;
                existingCarTrip.PlateNumber = carTripDto.PlateNumber;
                existingCarTrip.ArrivedTime = carTripDto.ArrivedTime;
                existingCarTrip.Status = carTripDto.Status;

                _unitOfWork.CarTripRepository.Update(existingCarTrip);
                await _unitOfWork.SaveAsync();

                return Ok(existingCarTrip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating car trip with TripId {tripId}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class CarTripDto
    {
        public int? TripId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string PlateNumber { get; set; }
        public TimeOnly? ArrivedTime { get; set; }
        public int? Status { get; set; }
    }

    public class CarTripCreateDto
    {
        [Required]
        public int? TripId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string PlateNumber { get; set; }
        public TimeOnly? ArrivedTime { get; set; }
        public int? Status { get; set; }
    }

    public class CarTripUpdateDto
    {
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string PlateNumber { get; set; }
        public TimeOnly? ArrivedTime { get; set; }
        public int? Status { get; set; }
    }
}