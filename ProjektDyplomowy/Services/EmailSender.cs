using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using ProjektDyplomowy.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class GmailEmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public GmailEmailSender(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.AppPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        return client.SendMailAsync(mailMessage);
    }
}