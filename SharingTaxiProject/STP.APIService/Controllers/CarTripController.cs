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

        /// <summary>
        /// API to retrieve all car trips.
        /// </summary>
        /// <returns>List of car trips with basic information.</returns>
        // GET: api/CarTrip
        [HttpGet]
        public async Task<IActionResult> GetCarTrips()
        {
            _logger.LogInformation("Getting all car trips.");

            // BƯỚC 1: Lấy tất cả car trip từ database
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

            // BƯỚC 2: Trả về danh sách car trip
            return Ok(carTrips);
        }

        /// <summary>
        /// API to retrieve a car trip by trip ID.
        /// </summary>
        /// <param name="tripId">ID of the car trip to retrieve.</param>
        /// <returns>Details of the specified car trip.</returns>
        // GET: api/CarTrip/{tripId}
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetCarTrip(int? tripId)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của tripId
            if (!tripId.HasValue)
            {
                return BadRequest("TripId must be provided.");
            }

            _logger.LogInformation($"Getting car trip with TripId: {tripId}");

            // BƯỚC 2: Truy vấn car trip từ database
            var carTrip = await _unitOfWork.CarTripRepository.GetByTripIdAsync(tripId.Value);
            if (carTrip == null)
            {
                _logger.LogWarning($"Car trip with TripId {tripId} not found.");
                return NotFound("Car trip not found.");
            }

            // BƯỚC 3: Tạo DTO và trả về kết quả
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

        /// <summary>
        /// API to create a new car trip.
        /// </summary>
        /// <param name="carTripDto">The details of the car trip to create.</param>
        /// <returns>The created car trip information.</returns>
        // POST: api/CarTrip
        [HttpPost]
        public async Task<IActionResult> CreateCarTrip([FromBody] CarTripCreateDto carTripDto)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của mô hình
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating a car trip.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Creating new car trip for TripId: {carTripDto.TripId}");

                // BƯỚC 2: Tạo đối tượng car trip mới từ DTO
                var carTrip = new CarTrip
                {
                    TripId = carTripDto.TripId,
                    DriverName = carTripDto.DriverName,
                    DriverPhone = carTripDto.DriverPhone,
                    PlateNumber = carTripDto.PlateNumber,
                    ArrivedTime = carTripDto.ArrivedTime,
                    Status = carTripDto.Status
                };

                // BƯỚC 3: Lưu car trip vào database
                await _unitOfWork.CarTripRepository.AddAsync(carTrip);
                await _unitOfWork.SaveAsync();

                // BƯỚC 4: Trả về car trip đã tạo
                return CreatedAtAction(nameof(GetCarTrip), new { tripId = carTrip.TripId }, carTrip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a car trip.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// API to update an existing car trip by trip ID.
        /// </summary>
        /// <param name="tripId">ID of the car trip to update.</param>
        /// <param name="carTripDto">The updated details of the car trip.</param>
        /// <returns>The updated car trip information.</returns>
        // PUT: api/CarTrip/{tripId}
        [HttpPut("{tripId}")]
        public async Task<IActionResult> UpdateCarTrip(int? tripId, [FromBody] CarTripUpdateDto carTripDto)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của tripId và mô hình
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

                // BƯỚC 2: Truy vấn car trip hiện có từ database
                var existingCarTrip = await _unitOfWork.CarTripRepository.GetByTripIdAsync(tripId.Value);
                if (existingCarTrip == null)
                {
                    _logger.LogWarning($"Car trip with TripId {tripId} not found during update.");
                    return NotFound("Car trip not found.");
                }

                // BƯỚC 3: Cập nhật thông tin car trip từ DTO
                existingCarTrip.DriverName = carTripDto.DriverName;
                existingCarTrip.DriverPhone = carTripDto.DriverPhone;
                existingCarTrip.PlateNumber = carTripDto.PlateNumber;
                existingCarTrip.ArrivedTime = carTripDto.ArrivedTime;
                existingCarTrip.Status = carTripDto.Status;

                // BƯỚC 4: Lưu các thay đổi vào database
                _unitOfWork.CarTripRepository.Update(existingCarTrip);
                await _unitOfWork.SaveAsync();

                // BƯỚC 5: Trả về car trip đã cập nhật
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
