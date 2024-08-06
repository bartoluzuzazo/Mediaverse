using MailKit.Net.Smtp;
using MailKit.Security;
using MediaVerse.Client.Application.Services.Emails;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace MediaVerse.Infrastructure.Emails;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Send(string emailBody, string recipientAddress, string subject)
    {
        var email = new MimeMessage();
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = emailBody
        };
        email.To.Add(MailboxAddress.Parse(recipientAddress));

        var emailSection = _configuration.GetSection("Email");
        email.From.Add(MailboxAddress.Parse(emailSection.GetSection("Username").Value));


        using var smtp = new SmtpClient();
        smtp.Connect(emailSection.GetSection("Host").Value, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(emailSection.GetSection("Username").Value, emailSection.GetSection("Password").Value);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}