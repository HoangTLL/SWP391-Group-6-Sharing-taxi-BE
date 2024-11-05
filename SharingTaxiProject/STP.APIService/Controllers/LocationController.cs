using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
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
                AreaId = l.AreaId,
                Status = l.Status
            });
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchLocations([FromQuery] string name)
        {
            var locations = await _unitOfWork.LocationRepository.GetLocationsByNameAsync(name);

            if (locations == null || !locations.Any())
            {
                return NotFound("No locations found with the specified name.");
            }

            var result = locations.Select(l => new
            {
                Id = l.Id,
                Name = l.Name,
                Lat = l.Lat,
                Lon = l.Lon,
                AreaId = l.AreaId,
                Status = l.Status
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<object>> AddLocation(Location location)
        {
            await _unitOfWork.LocationRepository.CreateAsync(location);
            await _unitOfWork.SaveAsync();

            var result = new
            {
                Id = location.Id,
                Name = location.Name,
                Lat = location.Lat,
                Lon = location.Lon,
                AreaId = location.AreaId,
                Status = location.Status
            };
            return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateLocation(int id, LocationUpdateDto locationUpdateDto)
        {
            var existingLocation = await _unitOfWork.LocationRepository.GetByIdAsync(id);

            if (existingLocation == null)
            {
                return NotFound();
            }

            // Update only the provided fields
            if (locationUpdateDto.Name != null) existingLocation.Name = locationUpdateDto.Name;
            if (locationUpdateDto.Lat.HasValue) existingLocation.Lat = locationUpdateDto.Lat.Value;
            if (locationUpdateDto.Lon.HasValue) existingLocation.Lon = locationUpdateDto.Lon.Value;
            if (locationUpdateDto.AreaId.HasValue) existingLocation.AreaId = locationUpdateDto.AreaId.Value;
            if (locationUpdateDto.Status.HasValue) existingLocation.Status = locationUpdateDto.Status.Value;

            await _unitOfWork.LocationRepository.UpdateAsync(existingLocation);
            await _unitOfWork.SaveAsync();

            var result = new
            {
                Id = existingLocation.Id,
                Name = existingLocation.Name,
                Lat = existingLocation.Lat,
                Lon = existingLocation.Lon,
                AreaId = existingLocation.AreaId,
                Status = existingLocation.Status
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteLocation(int id)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            await _unitOfWork.LocationRepository.RemoveAsync(location);
            await _unitOfWork.SaveAsync();

            var result = new
            {
                Id = location.Id,
                Name = location.Name,
                Lat = location.Lat,
                Lon = location.Lon,
                AreaId = location.AreaId,
                Status = location.Status
            };
            return Ok(result);
        }
    }
    public class LocationUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lon { get; set; }
        public int? AreaId { get; set; }
        public int? Status { get; set; }
    }
}