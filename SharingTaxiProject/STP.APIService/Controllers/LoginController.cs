using Microsoft.AspNetCore.Mvc;
using STP.Repository; // Tham chiếu đến lớp UnitOfWork và các repository
using STP.Repository.Models; // Tham chiếu đến các model trong dự án
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    /// <summary>
    /// API controller for handling user login operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // UnitOfWork để quản lý các repository và truy cập dữ liệu
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor for LoginController, injects UnitOfWork to access repositories.
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork instance to manage data access.</param>
        public LoginController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        /// <summary>
        /// HTTP POST method to authenticate a user and perform login.
        /// </summary>
        /// <param name="model">Login credentials (Email and Password).</param>
        /// <returns>Login success message and user details, or an error message.</returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ của dữ liệu nhận từ client
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi 400 nếu dữ liệu không hợp lệ
            }

            // BƯỚC 2: Tìm kiếm người dùng dựa trên email và mật khẩu
            var user = await _unitOfWork.UserRepository.GetByEmailAndPasswordAsync(model.Email, model.Password);

            // BƯỚC 3: Nếu không tìm thấy người dùng, trả về lỗi 401 (Unauthorized)
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // BƯỚC 4: Kiểm tra nếu tài khoản người dùng không ở trạng thái active (status = 1)
            if (user.Status != 1)
            {
                return Unauthorized("Your account is not active. Please contact support.");
            }

            // BƯỚC 5: Trả về thông báo đăng nhập thành công và chi tiết người dùng
            return Ok(new { message = "Login successful", userId = user.Id, role = user.Role });
        }
    }

    /// <summary>
    /// Model representing login data received from client.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; }
    }
}
