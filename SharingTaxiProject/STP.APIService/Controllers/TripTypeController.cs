using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using STP.Repository;
using STP.Repository.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace STP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripTypeController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<TripTypeController> _logger;

        public TripTypeController(UnitOfWork unitOfWork, ILogger<TripTypeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/TripType
        [HttpGet]
        public async Task<IActionResult> GetTripTypes()
        {
            _logger.LogInformation("Fetching all trip types.");

            var tripTypes = (await _unitOfWork.TripTypeRepository.GetAllAsync())
                .Select(tt => new
                {
                    tt.Id,
                    tt.Name,
                    tt.Description,
                    tt.FromAreaId,
                    tt.ToAreaId,
                    tt.BasicePrice,
                    tt.Status
                })
                .ToList();

            return Ok(tripTypes);
        }

        // GET: api/TripType/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripType(int id)
        {
            _logger.LogInformation($"Fetching trip type with ID {id}");

            var tripType = await _unitOfWork.TripTypeRepository.GetByIdAsync(id);
            if (tripType == null)
            {
                _logger.LogWarning($"Trip type with ID {id} not found.");
                return NotFound("Trip type not found.");
            }

            return Ok(new
            {
                tripType.Id,
                tripType.Name,
                tripType.Description,
                tripType.FromAreaId,
                tripType.ToAreaId,
                tripType.BasicePrice,
                tripType.Status
            });
        }

        // POST: api/TripType
        [HttpPost]
        public async Task<IActionResult> CreateTripType([FromBody] TripTypeCreateDto tripTypeDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input for creating trip type.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Creating a new trip type: {tripTypeDto.Name}");

                // Check if FromAreaId and ToAreaId exist
                if (!await _unitOfWork.TripTypeRepository.AreaExistsAsync(tripTypeDto.FromAreaId))
                {
                    return BadRequest($"FromAreaId {tripTypeDto.FromAreaId} does not exist.");
                }

                if (!await _unitOfWork.TripTypeRepository.AreaExistsAsync(tripTypeDto.ToAreaId))
                {
                    return BadRequest($"ToAreaId {tripTypeDto.ToAreaId} does not exist.");
                }

                // Check for duplicate trip types with the same FromAreaId and ToAreaId
                if (await _unitOfWork.TripTypeRepository.TripTypeExistsAsync(tripTypeDto.FromAreaId, tripTypeDto.ToAreaId))
                {
                    return BadRequest("A trip type with the same FromAreaId and ToAreaId already exists.");
                }

                var newTripType = new TripType
                {
                    Name = tripTypeDto.Name,
                    Description = tripTypeDto.Description,
                    FromAreaId = tripTypeDto.FromAreaId,
                    ToAreaId = tripTypeDto.ToAreaId,
                    BasicePrice = tripTypeDto.BasicePrice,
                    Status = 1
                };

                await _unitOfWork.TripTypeRepository.CreateAsync(newTripType);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(GetTripType), new { id = newTripType.Id }, newTripType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating trip type.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // PUT: api/TripType/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTripType(int id, [FromBody] TripTypeUpdateDto tripTypeDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid input for updating trip type with ID {id}.");
                return BadRequest(ModelState);
            }

            try
            {
                var existingTripType = await _unitOfWork.TripTypeRepository.GetByIdAsync(id);
                if (existingTripType == null)
                {
                    _logger.LogWarning($"Trip type with ID {id} not found.");
                    return NotFound("Trip type not found.");
                }

                // Check if FromAreaId and ToAreaId exist
                if (!await _unitOfWork.TripTypeRepository.AreaExistsAsync(tripTypeDto.FromAreaId))
                {
                    return BadRequest($"FromAreaId {tripTypeDto.FromAreaId} does not exist.");
                }

                if (!await _unitOfWork.TripTypeRepository.AreaExistsAsync(tripTypeDto.ToAreaId))
                {
                    return BadRequest($"ToAreaId {tripTypeDto.ToAreaId} does not exist.");
                }

                // Ensure no duplicate trip types with same FromAreaId and ToAreaId (except for the current one)
                if (await _unitOfWork.TripTypeRepository.TripTypeExistsAsync(tripTypeDto.FromAreaId, tripTypeDto.ToAreaId, id))
                {
                    return BadRequest("A trip type with the same FromAreaId and ToAreaId already exists.");
                }

                // Update existing trip type
                existingTripType.Name = tripTypeDto.Name;
                existingTripType.Description = tripTypeDto.Description;
                existingTripType.FromAreaId = tripTypeDto.FromAreaId;
                existingTripType.ToAreaId = tripTypeDto.ToAreaId;
                existingTripType.BasicePrice = tripTypeDto.BasicePrice;
                existingTripType.Status = tripTypeDto.Status;

                _unitOfWork.TripTypeRepository.Update(existingTripType);
                await _unitOfWork.SaveAsync();

                // Create response with only the required fields
                var updatedTripTypeDto = new
                {
                    id = existingTripType.Id,
                    fromAreaId = existingTripType.FromAreaId,
                    toAreaId = existingTripType.ToAreaId,
                    name = existingTripType.Name,
                    description = existingTripType.Description,
                    basicePrice = existingTripType.BasicePrice,
                    status = existingTripType.Status
                };

                return Ok(updatedTripTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating trip type with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

    }

    public class TripTypeCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int FromAreaId { get; set; }
        public int ToAreaId { get; set; }
        public decimal BasicePrice { get; set; }
    }

    public class TripTypeUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int FromAreaId { get; set; }
        public int ToAreaId { get; set; }
        public decimal BasicePrice { get; set; }
        public int? Status { get; set; }
    }
}
