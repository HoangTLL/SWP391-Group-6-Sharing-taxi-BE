﻿using Microsoft.AspNetCore.Mvc;
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
                // Create new user without manually setting the Id
                var newUser = new User
                {
                    Email = userSignUpDto.Email,
                    Password = userSignUpDto.Password,
                    CreatedAt = utcPlus7,
                    Role = "user"
                };

                await _userRepository.CreateAsync(newUser);

                return Ok(new
                {
                    message = "User signed up successfully.",
                    userId = newUser.Id // Return the automatically generated user ID
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
