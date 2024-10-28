using Microsoft.AspNetCore.Mvc;
using STP.Repository; // Tham chiếu đến UnitOfWork và LocationRepository
using STP.Repository.Models; // Tham chiếu đến model Location
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public LocationController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // API để lấy danh sách các điểm Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetLocations()
        {
            var locations = await _unitOfWork.LocationRepository.GetAllAsync();
            var result = locations.Select(l => new
            {
                Id = l.Id,
                Name = l.Name,
                Lat = l.Lat,
                Lon = l.Lon,
                AreaId = l.AreaId
            });
            return Ok(result);
        }

        // API để tìm kiếm các điểm Location theo tên
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Location>>> SearchLocations([FromQuery] string name)
        {
            var locations = await _unitOfWork.LocationRepository.GetLocationsByNameAsync(name);
            if (locations == null || !locations.Any())
            {
                return NotFound("No locations found with the specified name.");
            }
            return Ok(locations);
        }

        // API để thêm một Location mới
        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation(Location location)
        {
            await _unitOfWork.LocationRepository.CreateAsync(location);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
        }

        // API để cập nhật một Location
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            await _unitOfWork.LocationRepository.UpdateAsync(location);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // API để xóa một Location
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            await _unitOfWork.LocationRepository.RemoveAsync(location);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
