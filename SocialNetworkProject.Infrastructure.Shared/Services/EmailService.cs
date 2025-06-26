using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SocialNetworkProject.Core.Application.Dtos.Email;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Domain.Settings;

namespace SocialNetworkProject.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequestDto emailRequestDto)
        {
            try
            {
                MimeMessage email = new()
                {
                    Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom),
                    Subject = emailRequestDto.Subject
                };

                if (emailRequestDto.ToRange != null && emailRequestDto.ToRange.Any())
                {
                    foreach (var toItem in emailRequestDto.ToRange)
                    {
                        email.To.Add(MailboxAddress.Parse(toItem));
                    }
                }
                else
                {
                    email.To.Add(MailboxAddress.Parse(emailRequestDto.To));
                }

                BodyBuilder builder = new()
                {
                    HtmlBody = emailRequestDto.HtmlBody
                };
                email.Body = builder.ToMessageBody();

                using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
                await smtpClient.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtpClient.SendAsync(email);
                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió una excepción al enviar el correo: {Exception}", ex);
            }
        }
    }
}
