using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using STP.Repository;
using STP.Repository.Models;
using System.Security.Cryptography;
using System.Text;

namespace STP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;

        public PasswordResetController(IConfiguration configuration, UnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequestDto request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                // Return a generic message to avoid revealing email existence
                return Ok(new { message = "Nếu địa chỉ email tồn tại, một email chứa mật khẩu mới sẽ được gửi." });
            }

            string newPassword = GenerateRandomPassword();
            user.Password = newPassword; // Save the new password directly without hashing

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            await SendNewPasswordEmail(user.Email, newPassword);

            return Ok(new { message = "Nếu địa chỉ email tồn tại, một email chứa mật khẩu mới đã được gửi." });
        }


        private string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        

        private async Task SendNewPasswordEmail(string email, string newPassword)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Mật khẩu mới của bạn";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <h2>Mật khẩu mới của bạn</h2>
                <p>Mật khẩu mới của bạn là: <strong>{newPassword}</strong></p>
                <p>Vui lòng đăng nhập và thay đổi mật khẩu này ngay sau khi bạn đăng nhập thành công.</p>
                <p>Vì lý do bảo mật, chúng tôi khuyên bạn nên thay đổi mật khẩu này ngay lập tức sau khi đăng nhập.</p>";

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["SmtpSettings:Server"], int.Parse(_configuration["SmtpSettings:Port"]), bool.Parse(_configuration["SmtpSettings:UseSSL"]));
            await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public class PasswordResetRequestDto
    {
        public string Email { get; set; }
    }
}