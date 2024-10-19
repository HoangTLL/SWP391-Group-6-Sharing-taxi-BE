using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService(string smtpServer, int port, string username, string password)
    {
        _smtpClient = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mailMessage = new MailMessage("hoanghoangtllse182292@fpt.edu.vn", to, subject, body);
        mailMessage.IsBodyHtml = true;

        await _smtpClient.SendMailAsync(mailMessage);
    }
}
