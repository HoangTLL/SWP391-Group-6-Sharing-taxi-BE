using Microsoft.AspNetCore.Mvc;
using STP.Repository; // Tham chiếu đến lớp UnitOfWork và các repository
using STP.Repository.Models; // Tham chiếu đến model Area trong dự án
using System.Collections.Generic; // Thư viện để sử dụng IEnumerable
using System.Threading.Tasks; // Thư viện để hỗ trợ các phương thức bất đồng bộ

namespace STP.APIService.Controllers
{
    // Định nghĩa API controller cho thao tác với các thực thể Area
    [Route("api/[controller]")] // Định nghĩa route: api/Area
    [ApiController] // Đánh dấu đây là một API controller
    public class AreaController : ControllerBase
    {
        // Sử dụng UnitOfWork để quản lý các repository và truy cập cơ sở dữ liệu
        private readonly UnitOfWork _unitOfWork;

        // Constructor của AreaController, inject UnitOfWork để có thể sử dụng các repository
        public AreaController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        // Phương thức GET để lấy danh sách các Area (khu vực) từ cơ sở dữ liệu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
        {
            // Sử dụng AreaRepository từ UnitOfWork để lấy tất cả các Area một cách bất đồng bộ
            var areas = await _unitOfWork.  AreaRepository.GetAllAsync();
            return Ok(areas);
        }
    }
}
