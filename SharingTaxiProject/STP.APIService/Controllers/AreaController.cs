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
    public class AreaController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<AreaController> _logger;

        public AreaController(UnitOfWork unitOfWork, ILogger<AreaController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/Area
        [HttpGet]
        public async Task<IActionResult> GetAreas()
        {
            _logger.LogInformation("Getting all areas.");

            var areas = (await _unitOfWork.AreaRepository.GetAllAsync())
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Description,
                    a.Status
                })
                .ToList();

            return Ok(areas);
        }

        // GET: api/Area/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArea(int id)
        {
            _logger.LogInformation($"Getting area with ID: {id}");

            var area = await _unitOfWork.AreaRepository.GetByIdAsync(id);
            if (area == null)
            {
                _logger.LogWarning($"Area with ID {id} not found.");
                return NotFound("Area not found.");
            }

            var areaDto = new
            {
                area.Id,
                area.Name,
                area.Description,
                area.Status
            };

            return Ok(areaDto);
        }

        // POST: api/Area
        [HttpPost]
        public async Task<IActionResult> CreateArea([FromBody] AreaCreateDto areaDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating an area.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Creating new area: {areaDto.Name}");

                var area = new Area
                {
                    Name = areaDto.Name,
                    Description = areaDto.Description,
                    Status = 1
                };

                await _unitOfWork.AreaRepository.AddAsync(area);
                await _unitOfWork.SaveAsync();

                var createdAreaDto = new
                {
                    area.Id,
                    area.Name,
                    area.Description,
                    area.Status
                };

                return CreatedAtAction(nameof(GetArea), new { id = area.Id }, createdAreaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an area.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Area/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] AreaUpdateDto areaDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid model state during update for area ID {id}.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Updating area with ID: {id}");
                var existingArea = await _unitOfWork.AreaRepository.GetByIdAsync(id);
                if (existingArea == null)
                {
                    _logger.LogWarning($"Area with ID {id} not found during update.");
                    return NotFound("Area not found.");
                }

                existingArea.Name = areaDto.Name;
                existingArea.Description = areaDto.Description;
                existingArea.Status = areaDto.Status;

                _unitOfWork.AreaRepository.Update(existingArea);
                await _unitOfWork.SaveAsync();

                var updatedAreaDto = new
                {
                    existingArea.Id,
                    existingArea.Name,
                    existingArea.Description,
                    existingArea.Status
                };

                return Ok(updatedAreaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating area with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class AreaCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
    }

    public class AreaUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
    }
}