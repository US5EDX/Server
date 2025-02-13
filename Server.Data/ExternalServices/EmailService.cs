using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Server.Models.Interfaces;
using Server.Services.Dtos;

namespace Server.Data.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly MailOptions _emailOptions;

        public EmailService(IOptions<MailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SmtpUser));
            emailMessage.To.Add(new MailboxAddress("Отримувач", to));
            emailMessage.Subject = subject;

            var bodyPart = new TextPart("plain")
            {
                Text = body
            };

            emailMessage.Body = bodyPart;

            using (var client = new SmtpClient())
            {
                client.Timeout = 10000;
                await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, useSsl: true);
                await client.AuthenticateAsync(_emailOptions.SmtpUser, _emailOptions.SmtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
