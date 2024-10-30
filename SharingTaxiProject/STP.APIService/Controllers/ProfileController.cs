using Microsoft.AspNetCore.Mvc;
using STP.Repository; // Tham chiếu đến UnitOfWork
using STP.Repository.Models; // Tham chiếu đến model User
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    /// <summary>
    /// API controller for managing user profiles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor for ProfileController, injects UnitOfWork for data operations.
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork instance for database access.</param>
        public ProfileController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        /// <summary>
        /// API to retrieve a user's profile based on their user ID.
        /// </summary>
        /// <param name="userId">ID of the user whose profile is requested.</param>
        /// <returns>User profile information or a not found message if the user does not exist.</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetProfile(int userId)
        {
            // BƯỚC 1: Tìm kiếm người dùng dựa trên userId
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            // BƯỚC 2: Kiểm tra nếu không tìm thấy người dùng, trả về lỗi 404
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // BƯỚC 3: Tạo DTO với thông tin cần thiết của người dùng
            var userProfile = new
            {
                Email = user.Email,
                Name = user.Name,
                Phone = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt
            };

            // BƯỚC 4: Trả về đối tượng thông tin người dùng
            return Ok(userProfile);
        }
    }
}
