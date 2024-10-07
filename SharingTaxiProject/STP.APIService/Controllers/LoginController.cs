using Microsoft.AspNetCore.Mvc;
using STP.Repository; // Tham chiếu đến lớp UnitOfWork và các repository
using STP.Repository.Models; // Tham chiếu đến các model trong dự án
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    // Định nghĩa API controller cho login
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // UnitOfWork để quản lý các repository và truy cập dữ liệu
        private readonly UnitOfWork _unitOfWork;

        // Constructor của LoginController, inject UnitOfWork để sử dụng các repository
        public LoginController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        // Phương thức HTTP POST để thực hiện đăng nhập
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            // Kiểm tra tính hợp lệ của dữ liệu nhận được từ client
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi 400 nếu dữ liệu không hợp lệ
            }

            // Gọi repository để tìm người dùng dựa trên email và password
            var user = await _unitOfWork.UserRepository.GetByEmailAndPasswordAsync(model.Email, model.Password);

            // Nếu không tìm thấy người dùng, trả về lỗi 401 (Unauthorized)
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // Nếu thành công, trả về thông báo và ID người dùng
            return Ok(new { message = "Login successful", userId = user.Id, role = user.Role });
        }
    }

    // Model đại diện cho dữ liệu login từ client
    public class LoginModel
    {
        // Thuộc tính Email để nhận từ request body
        public string Email { get; set; }

        // Thuộc tính Password để nhận từ request body
        public string Password { get; set; }
    }
}
