namespace MediaVerse.Client.Application.Services.Emails;

public interface IEmailService
{
    void Send(string emailBody, string recipientAddress, string subject);
}