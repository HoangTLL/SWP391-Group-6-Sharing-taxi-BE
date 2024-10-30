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

        /// <summary>
        /// API to retrieve all areas.
        /// </summary>
        /// <returns>List of areas with basic information.</returns>
        // GET: api/Area
        [HttpGet]
        public async Task<IActionResult> GetAreas()
        {
            _logger.LogInformation("Getting all areas.");

            // BƯỚC 1: Lấy tất cả các vùng từ database
            var areas = (await _unitOfWork.AreaRepository.GetAllAsync())
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Description,
                    a.Status
                })
                .ToList();

            // BƯỚC 2: Trả về danh sách vùng
            return Ok(areas);
        }

        /// <summary>
        /// API to retrieve a single area by ID.
        /// </summary>
        /// <param name="id">The ID of the area to retrieve.</param>
        /// <returns>Details of the requested area.</returns>
        // GET: api/Area/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArea(int id)
        {
            _logger.LogInformation($"Getting area with ID: {id}");

            // BƯỚC 1: Truy vấn vùng từ database theo ID
            var area = await _unitOfWork.AreaRepository.GetByIdAsync(id);

            // BƯỚC 2: Nếu không tìm thấy vùng, trả về lỗi 404
            if (area == null)
            {
                _logger.LogWarning($"Area with ID {id} not found.");
                return NotFound("Area not found.");
            }

            // BƯỚC 3: Tạo DTO cho vùng và trả về kết quả
            var areaDto = new
            {
                area.Id,
                area.Name,
                area.Description,
                area.Status
            };

            return Ok(areaDto);
        }

        /// <summary>
        /// API to create a new area.
        /// </summary>
        /// <param name="areaDto">The details of the area to create.</param>
        /// <returns>The created area information.</returns>
        // POST: api/Area
        [HttpPost]
        public async Task<IActionResult> CreateArea([FromBody] AreaCreateDto areaDto)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của mô hình
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating an area.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Creating new area: {areaDto.Name}");

                // BƯỚC 2: Tạo đối tượng vùng mới từ DTO
                var area = new Area
                {
                    Name = areaDto.Name,
                    Description = areaDto.Description,
                    Status = 1
                };

                // BƯỚC 3: Lưu vùng vào database
                await _unitOfWork.AreaRepository.AddAsync(area);
                await _unitOfWork.SaveAsync();

                // BƯỚC 4: Trả về vùng đã tạo
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

        /// <summary>
        /// API to update an existing area by ID.
        /// </summary>
        /// <param name="id">The ID of the area to update.</param>
        /// <param name="areaDto">The updated details of the area.</param>
        /// <returns>The updated area information.</returns>
        // PUT: api/Area/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] AreaUpdateDto areaDto)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của mô hình
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid model state during update for area ID {id}.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Updating area with ID: {id}");

                // BƯỚC 2: Truy vấn vùng hiện có từ database
                var existingArea = await _unitOfWork.AreaRepository.GetByIdAsync(id);

                // BƯỚC 3: Nếu không tìm thấy vùng, trả về lỗi 404
                if (existingArea == null)
                {
                    _logger.LogWarning($"Area with ID {id} not found during update.");
                    return NotFound("Area not found.");
                }

                // BƯỚC 4: Cập nhật thông tin vùng theo DTO
                existingArea.Name = areaDto.Name;
                existingArea.Description = areaDto.Description;
                existingArea.Status = areaDto.Status;

                // BƯỚC 5: Lưu các thay đổi vào database
                _unitOfWork.AreaRepository.Update(existingArea);
                await _unitOfWork.SaveAsync();

                // BƯỚC 6: Trả về vùng đã cập nhật
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
