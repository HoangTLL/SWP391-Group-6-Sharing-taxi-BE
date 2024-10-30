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
    /// <summary>
    /// API controller for handling password reset functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor for PasswordResetController, initializes configuration and UnitOfWork.
        /// </summary>
        /// <param name="configuration">Application configuration settings.</param>
        /// <param name="unitOfWork">UnitOfWork instance to manage data access.</param>
        public PasswordResetController(IConfiguration configuration, UnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// API to reset the password for a user by email.
        /// </summary>
        /// <param name="request">The password reset request containing the user's email.</param>
        /// <returns>A success message indicating that a new password was sent if the email exists.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequestDto request)
        {
            // BƯỚC 1: Tìm kiếm người dùng theo email
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                // BƯỚC 2: Trả về thông báo không rõ ràng nếu email không tồn tại để đảm bảo bảo mật
                return Ok(new { message = "Nếu địa chỉ email tồn tại, một email chứa mật khẩu mới sẽ được gửi." });
            }

            // BƯỚC 3: Tạo mật khẩu mới ngẫu nhiên
            string newPassword = GenerateRandomPassword();
            user.Password = newPassword; // Lưu mật khẩu mới mà không mã hóa

            // BƯỚC 4: Cập nhật mật khẩu trong database
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            // BƯỚC 5: Gửi email chứa mật khẩu mới cho người dùng
            await SendNewPasswordEmail(user.Email, newPassword);

            return Ok(new { message = "Nếu địa chỉ email tồn tại, một email chứa mật khẩu mới đã được gửi." });
        }

        /// <summary>
        /// Generates a random password of specified length.
        /// </summary>
        /// <param name="length">The length of the password to generate.</param>
        /// <returns>A randomly generated password.</returns>
        private string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Sends an email to the user with their new password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="newPassword">The new password to send to the user.</param>
        private async Task SendNewPasswordEmail(string email, string newPassword)
        {
            // Tạo email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Mật khẩu mới của bạn";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <h2>Mật khẩu mới của bạn</h2>
                <p>Mật khẩu mới của bạn là: <strong>{newPassword}</strong></p>
                <p>Vui lòng đăng nhập và thay đổi mật khẩu này ngay sau khi bạn đăng nhập thành công.</p>
                <p>Vì lý do bảo mật, chúng tôi khuyên bạn nên thay đổi mật khẩu này ngay lập tức sau khi đăng nhập.</p>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            // Gửi email
            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["SmtpSettings:Server"], int.Parse(_configuration["SmtpSettings:Port"]), bool.Parse(_configuration["SmtpSettings:UseSSL"]));
            await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    /// <summary>
    /// DTO for requesting a password reset.
    /// </summary>
    public class PasswordResetRequestDto
    {
        /// <summary>
        /// The email address of the user requesting the password reset.
        /// </summary>
        public string Email { get; set; }
    }
}
