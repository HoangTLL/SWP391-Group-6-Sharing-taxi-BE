using Microsoft.AspNetCore.Mvc;
using STP.Repository; // Tham chiếu đến UnitOfWork
using STP.Repository.Models; // Tham chiếu đến model User
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        // Inject UnitOfWork để quản lý các thao tác với cơ sở dữ liệu
        public ProfileController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        // API lấy thông tin hồ sơ người dùng theo userId
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetProfile(int userId)
        {
            // Tìm kiếm người dùng dựa trên userId
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            // Kiểm tra xem người dùng có tồn tại không
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Trả về thông tin cần thiết của người dùng
            var userProfile = new
            {
                Email = user.Email,
                Name = user.Name,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt
            };

            return Ok(userProfile); // Trả về đối tượng đã xử lý
        }
    }
}
