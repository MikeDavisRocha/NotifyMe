using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NotifyMe.Models;
using NotifyMe.Data;

namespace NotifyMe.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(EmailRequest request)
    {
        var emailLog = new EmailLog
        {
            Recipient = request.To,
            Subject = request.Subject,
            Body = request.Body,
            SentAt = DateTime.UtcNow
        };

        try
        {
            var useMock = _configuration.GetValue<bool>("Smtp:UseMock");
            if (useMock)
            {
                // Simulate network delay
                await Task.Delay(1000);
                Console.WriteLine($"[Mock Email] To: {request.To}, Subject: {request.Subject}");
                emailLog.Success = true;
                // Skip SMTP logic
            }
            else
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("NotifyMe", _configuration["Smtp:From"] ?? "noreply@notifyme.com"));
                message.To.Add(new MailboxAddress("", request.To));
                message.Subject = request.Subject;
                message.Body = new TextPart("plain") { Text = request.Body };

                using var client = new SmtpClient();
                // For demo purposes, accepting all certs. In prod, validate properly.
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                var host = _configuration["Smtp:Host"] ?? "localhost";
                var port = int.Parse(_configuration["Smtp:Port"] ?? "587");
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];

                await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                
                if (!string.IsNullOrEmpty(username))
                {
                    await client.AuthenticateAsync(username, password);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                emailLog.Success = true;
            }
        }
        catch (Exception ex)
        {
            emailLog.Success = false;
            emailLog.ErrorMessage = ex.Message;
        }

        // Save log using a new scope
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.EmailLogs.Add(emailLog);
        await dbContext.SaveChangesAsync();
    }
}
