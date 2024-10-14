using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _port = 587;
    private readonly string _username;
    private readonly string _password;

    public EmailService(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using (var smtpClient = new SmtpClient(_smtpServer))
        {
            smtpClient.Port = _port;
            smtpClient.Credentials = new NetworkCredential(_username, _password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

