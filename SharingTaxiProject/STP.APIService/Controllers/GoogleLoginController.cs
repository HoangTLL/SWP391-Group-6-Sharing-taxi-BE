using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Threading.Tasks;
using System.Security.Claims;
using Google.Apis.Auth;
using STP.Repository.Models;
using STP.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace STP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleLoginController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleLoginController> _logger;

        public GoogleLoginController(UnitOfWork unitOfWork, IConfiguration configuration, ILogger<GoogleLoginController> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            _logger.LogInformation("Initiating Google Sign-In");
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("google-callback") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Google authentication failed.");
                    return BadRequest("Google authentication failed.");
                }

                var googleUser = result.Principal;
                var email = googleUser.FindFirstValue(ClaimTypes.Email);
                var name = googleUser.FindFirstValue(ClaimTypes.Name);

                var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    user = await CreateNewUser(email, name);
                }

                var token = GenerateJwtToken(user);

                return Ok(new { token, userId = user.Id, email = user.Email, name = user.Name });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Google authentication.");
                return StatusCode(500, "An error occurred during authentication.");
            }
        }

        private async Task<User> CreateNewUser(string email, string name)
        {
            var user = new User
            {
                Email = email,
                Name = name,
                CreatedAt = DateTime.UtcNow,
                Role = "user",
                Status = 1
            };

            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.SaveAsync();

            var wallet = new Wallet
            {
                UserId = user.Id,
                Balance = 0,
                CurrencyCode = "VND",
                CreatedAt = DateTime.UtcNow,
                Status = 1
            };

            await _unitOfWork.WalletRepository.CreateAsync(wallet);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"New user created: {user.Email}");
            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}