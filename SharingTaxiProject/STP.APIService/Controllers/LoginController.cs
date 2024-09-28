using Microsoft.AspNetCore.Mvc;
using STP.Repository;
using STP.Repository.Models;
using System.Threading.Tasks;

namespace STP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public LoginController(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _unitOfWork.UserRepository.GetByEmailAndPasswordAsync(model.Email, model.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(new { message = "Login successful", userId = user.Id });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
