using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using STP.Repository;
using STP.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripTypePricingController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<TripTypePricingController> _logger;

        public TripTypePricingController(UnitOfWork unitOfWork, ILogger<TripTypePricingController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/TripTypePricing
        [HttpGet]
        public async Task<IActionResult> GetAllTripTypePricings()
        {
            var TripTypePricings = await _unitOfWork.TripTypePricingRepository.GetAllTripTypePricingsAsync();
            var result = MapToDto(TripTypePricings);
            return Ok(result);
        }

        // GET: api/TripTypePricing/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripTypePricingById(int id)
        {
            var TripTypePricing = await _unitOfWork.TripTypePricingRepository.GetTripTypePricingByIdAsync(id);
            if (TripTypePricing == null)
            {
                return NotFound("Tripe type pricing not found.");
            }

            var result = MapToDto(TripTypePricing);
            return Ok(result);
        }

        // POST: api/TripTypePricing
        [HttpPost]
        public async Task<IActionResult> CreateTripTypePricing([FromBody] TripTypePricingCreateDto TripTypePricingDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input for creating tripe type pricing.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Creating a new tripe type pricing: {TripTypePricingDto.Name}");

                if (!await _unitOfWork.TripTypePricingRepository.TripTypeExistsAsync(TripTypePricingDto.TripType))
                {
                    return BadRequest($"TripType with ID {TripTypePricingDto.TripType} does not exist.");
                }

                // Check if a TripTypePricing with the same parameters already exists
                if (await _unitOfWork.TripTypePricingRepository.ExistsWithSameParametersAsync(
                    TripTypePricingDto.TripType, TripTypePricingDto.MinPerson, TripTypePricingDto.MaxPerson))
                {
                    return BadRequest("A TripTypePricing with the same TripType, MinPerson, and MaxPerson already exists.");
                }

                var newTripTypePricing = new TripTypePricing
                {
                    Name = TripTypePricingDto.Name,
                    TripType = TripTypePricingDto.TripType,
                    MinPerson = TripTypePricingDto.MinPerson,
                    MaxPerson = TripTypePricingDto.MaxPerson,
                    PricePerPerson = TripTypePricingDto.PricePerPerson,
                    Status = 1 // Assuming 1 means 'active'
                };

                await _unitOfWork.TripTypePricingRepository.CreateAsync(newTripTypePricing);
                await _unitOfWork.SaveAsync();

                var result = MapToDto(newTripTypePricing);
                return CreatedAtAction(nameof(GetTripTypePricingById), new { id = newTripTypePricing.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating tripe type pricing.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // PUT: api/TripTypePricing/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTripTypePricing(int id, [FromBody] TripTypePricingUpdateDto TripTypePricingDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid input for updating tripe type pricing with ID {id}.");
                return BadRequest(ModelState);
            }

            try
            {
                var existingTripTypePricing = await _unitOfWork.TripTypePricingRepository.GetTripTypePricingByIdAsync(id);
                if (existingTripTypePricing == null)
                {
                    _logger.LogWarning($"Tripe type pricing with ID {id} not found.");
                    return NotFound("Tripe type pricing not found.");
                }

                if (!await _unitOfWork.TripTypePricingRepository.TripTypeExistsAsync(TripTypePricingDto.TripType))
                {
                    return BadRequest($"TripType with ID {TripTypePricingDto.TripType} does not exist.");
                }

                // Check if another TripTypePricing with the same parameters exists (excluding the current one)
                if (await _unitOfWork.TripTypePricingRepository.ExistsWithSameParametersAsync(
                    TripTypePricingDto.TripType, TripTypePricingDto.MinPerson, TripTypePricingDto.MaxPerson, id))
                {
                    return BadRequest("Another TripTypePricing with the same TripType, MinPerson, and MaxPerson already exists.");
                }

                // Update existing tripe type pricing
                existingTripTypePricing.Name = TripTypePricingDto.Name;
                existingTripTypePricing.TripType = TripTypePricingDto.TripType;
                existingTripTypePricing.MinPerson = TripTypePricingDto.MinPerson;
                existingTripTypePricing.MaxPerson = TripTypePricingDto.MaxPerson;
                existingTripTypePricing.PricePerPerson = TripTypePricingDto.PricePerPerson;
                existingTripTypePricing.Status = TripTypePricingDto.Status;

                await _unitOfWork.TripTypePricingRepository.UpdateAsync(existingTripTypePricing);
                await _unitOfWork.SaveAsync();

                var result = MapToDto(existingTripTypePricing);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating tripe type pricing with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Helper method to map TripTypePricing to a DTO
        private object MapToDto(TripTypePricing TripTypePricing)
        {
            return new
            {
                id = TripTypePricing.Id,
                name = TripTypePricing.Name,
                tripType = TripTypePricing.TripType,
                minPerson = TripTypePricing.MinPerson,
                maxPerson = TripTypePricing.MaxPerson,
                pricePerPerson = TripTypePricing.PricePerPerson,
                status = TripTypePricing.Status
            };
        }

        // Helper method to map a list of TripTypePricing to DTOs
        private IEnumerable<object> MapToDto(IEnumerable<TripTypePricing> TripTypePricings)
        {
            foreach (var pricing in TripTypePricings)
            {
                yield return MapToDto(pricing);
            }
        }
    }

    public class TripTypePricingCreateDto
    {
        public string Name { get; set; }
        public int TripType { get; set; }
        public int MinPerson { get; set; }
        public int MaxPerson { get; set; }
        public decimal PricePerPerson { get; set; }
    }

    public class TripTypePricingUpdateDto
    {
        public string Name { get; set; }
        public int TripType { get; set; }
        public int MinPerson { get; set; }
        public int MaxPerson { get; set; }
        public decimal PricePerPerson { get; set; }
        public int? Status { get; set; }
    }
}
