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

        public UserController(UnitOfWork unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
        }

        // POST: api/User/SignUp
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
                    message = "Email is already registered.",

                });
            }

            var utcPlus7 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

            try
            {
                // Get the latest ID from the database
                var latestId = await _userRepository.GetLatestIdAsync();

                // Create new user with incremented ID
                var newUser = new User
                {
                    Id = latestId + 1, // Increment the latest ID
                    Email = userSignUpDto.Email,
                    Password = userSignUpDto.Password,
                    CreatedAt = utcPlus7,
                    Role = "user"
                };

                await _userRepository.CreateAsync(newUser);

                return Ok(new
                {
                    message = "User signed up successfully.",
                    userId = newUser.Id // Return the newly created user ID
                });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
        [HttpPut("{id}")]
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
    }

    public class UserSignUpDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
