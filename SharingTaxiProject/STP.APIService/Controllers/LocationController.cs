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

        /// <summary>
        /// API to retrieve a list of all locations.
        /// </summary>
        /// <returns>List of locations with basic details.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetLocations()
        {
            // BƯỚC 1: Lấy tất cả các địa điểm từ LocationRepository
            var locations = await _unitOfWork.LocationRepository.GetAllAsync();

            // BƯỚC 2: Chuyển đổi dữ liệu địa điểm thành định dạng DTO
            var result = locations.Select(l => new
            {
                Id = l.Id,
                Name = l.Name,
                Lat = l.Lat,
                Lon = l.Lon,
                AreaId = l.AreaId
            });

            // BƯỚC 3: Trả về danh sách địa điểm
            return Ok(result);
        }

        /// <summary>
        /// API to search locations by name.
        /// </summary>
        /// <param name="name">The name or partial name of locations to search.</param>
        /// <returns>List of locations matching the search criteria.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Location>>> SearchLocations([FromQuery] string name)
        {
            // BƯỚC 1: Tìm kiếm các địa điểm theo tên từ LocationRepository
            var locations = await _unitOfWork.LocationRepository.GetLocationsByNameAsync(name);

            // BƯỚC 2: Kiểm tra nếu không tìm thấy địa điểm nào
            if (locations == null || !locations.Any())
            {
                return NotFound("No locations found with the specified name.");
            }

            // BƯỚC 3: Trả về danh sách địa điểm tìm thấy
            return Ok(locations);
        }

        /// <summary>
        /// API to add a new location.
        /// </summary>
        /// <param name="location">Details of the location to add.</param>
        /// <returns>The added location details.</returns>
        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation(Location location)
        {
            // BƯỚC 1: Tạo mới một địa điểm trong LocationRepository
            await _unitOfWork.LocationRepository.CreateAsync(location);
            await _unitOfWork.SaveAsync();

            // BƯỚC 2: Trả về địa điểm đã tạo
            return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
        }

        /// <summary>
        /// API to update an existing location by its ID.
        /// </summary>
        /// <param name="id">ID of the location to update.</param>
        /// <param name="location">Updated details of the location.</param>
        /// <returns>Status of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của ID
            if (id != location.Id)
            {
                return BadRequest();
            }

            // BƯỚC 2: Cập nhật thông tin địa điểm trong LocationRepository
            await _unitOfWork.LocationRepository.UpdateAsync(location);
            await _unitOfWork.SaveAsync();

            // BƯỚC 3: Trả về trạng thái không có nội dung
            return NoContent();
        }

        /// <summary>
        /// API to delete a location by its ID.
        /// </summary>
        /// <param name="id">ID of the location to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            // BƯỚC 1: Truy vấn địa điểm từ database theo ID
            var location = await _unitOfWork.LocationRepository.GetByIdAsync(id);

            // BƯỚC 2: Kiểm tra nếu không tìm thấy địa điểm
            if (location == null)
            {
                return NotFound();
            }

            // BƯỚC 3: Xóa địa điểm khỏi LocationRepository
            await _unitOfWork.LocationRepository.RemoveAsync(location);
            await _unitOfWork.SaveAsync();

            // BƯỚC 4: Trả về trạng thái không có nội dung
            return NoContent();
        }
    }
}
