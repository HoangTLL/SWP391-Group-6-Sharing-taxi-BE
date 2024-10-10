using Microsoft.AspNetCore.Mvc;
using STP.Repository.Models;
using STP.Repository.Repository;
using STP.Repository;
using System.Threading.Tasks;

namespace STP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly WalletRepository _walletRepository;
        private readonly UnitOfWork _unitOfWork;

        public UserController(UnitOfWork unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
            _unitOfWork = unitOfWork;
            _walletRepository = unitOfWork.WalletRepository;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userSignUpDto)
        {
            if (userSignUpDto == null)
                return BadRequest("User data is null.");

            var existingUser = await _userRepository.GetByEmailAsync(userSignUpDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email is already registered."
                });
            }

            var utcPlus7 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            var dateOfBirth = DateOnly.FromDateTime(userSignUpDto.DateOfBirth);

            try
            {
                var newUser = new User
                {
                    // Không cần gán Id, SQL Server sẽ tự động tăng giá trị
                    Name = userSignUpDto.Name,                 // Gán tên từ DTO
                    Email = userSignUpDto.Email,               // Gán email
                    PhoneNumber = userSignUpDto.PhoneNumber,   // Gán số điện thoại
                    DateOfBirth = dateOfBirth,   // Gán ngày sinh
                    Password = userSignUpDto.Password,
                    CreatedAt = utcPlus7,                      // Gán thời gian tạo
                    Role = "user"                              // Gán role mặc định
                };

                // Tạo người dùng
                await _userRepository.CreateAsync(newUser);

                // Sau khi người dùng được tạo thành công, tạo ví điện tử cho người dùng
                var newWallet = new Wallet
                {
                    UserId = newUser.Id,      // Gán ví cho người dùng mới tạo
                    Balance = 0,              // Số dư ban đầu là 0
                    CurrencyCode = "VND",     // Hoặc loại tiền tệ khác
                    CreatedAt = utcPlus7,     // Sử dụng thời gian giống như khi tạo user
                    Status = 1                // Trạng thái Active
                };

                // Lưu ví vào cơ sở dữ liệu
                await _walletRepository.CreateAsync(newWallet);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _unitOfWork.SaveAsync();

                // Trả về kết quả
                return Ok(new
                {
                    message = "User signed up successfully.",
                    userId = newUser.Id,       // Trả về ID người dùng mới tạo
                    walletId = newWallet.Id    // Trả về ID của ví mới tạo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
                return BadRequest("User data is null.");

            var existingUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email is already registered.",
                });
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            // Update user properties
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Password = updateUserDto.Password; // Remember to hash passwords in production
            user.DateOfBirth = updateUserDto.DateOfBirth;

            try
            {
                await _userRepository.UpdateAsync(user);
                return Ok(new { message = "User updated successfully." }); // Return success message
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        // PUT: api/User/UpdateUserAdmin/{id}
        [HttpPut("UpdateUserAdmin/{id}")]
        public async Task<IActionResult> UpdateUserAdmin(int id, [FromBody] UpdateUserDtoForAdmin updateUserDto)
        {
            if (updateUserDto == null)
                return BadRequest("User data is null.");

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound("User not found.");

            // Check if the new email is already in use
            var emailCheck = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (emailCheck != null && emailCheck.Id != existingUser.Id)
            {
                return BadRequest(new
                {
                    message = "Email is already registered."
                });
            }

            // Create a new user with updated details
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            // Update user properties
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Password = updateUserDto.Password; // Remember to hash passwords in production
            user.DateOfBirth = updateUserDto.DateOfBirth;
            user.Role = updateUserDto.Role;
            try
            {
                await _userRepository.UpdateAsync(user);
                return Ok(new { message = "User updated successfully." }); // Return success message
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // GET: api/UserList
        [HttpGet("User List")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                // Fetch all users asynchronously
                var users = await _userRepository.GetAllAsync();

                // Select only the necessary fields
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Password = u.Password,
                    DateOfBirth = u.DateOfBirth,
                    CreatedAt = u.CreatedAt,
                    Role = u.Role
                }).ToList();

                // Return the list of user DTOs
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "An error occurred while fetching the users.");
            }
        }

    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Role { get; set; }
    }

    public class UserSignUpDto
    {
        public string Name { get; set; }           // Tên người dùng
        public string Email { get; set; }          // Email
        public string PhoneNumber { get; set; }    // Số điện thoại
        public DateTime DateOfBirth { get; set; }  // Ngày sinh
        public string Password { get; set; }       // Mật khẩu
    }

    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
    public class UpdateUserDtoForAdmin
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; } // Remember to hash passwords in production
        public DateOnly? DateOfBirth { get; set; }
        public string Role { get; set; } // New field for updating user role
    }
}
