using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;

namespace STP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public AreaController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> getAreaCategory()
        {
            return await _unitOfWork.areaRepository.GetAllAsync();
        }
    }
}
