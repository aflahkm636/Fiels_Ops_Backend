using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string htmlBody)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("SmartServeERP", _settings.From));
        email.To.Add(new MailboxAddress("", to));
        email.Subject = subject;

        email.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password 
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("SMTP ERROR: " + ex.Message);
            return false;
        }
    }
}
