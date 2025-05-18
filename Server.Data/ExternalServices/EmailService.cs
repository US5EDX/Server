using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Server.Models.Interfaces.ExternalInterfaces;
using Server.Services.Options.SettingsOptions;

namespace Server.Data.ExternalServices
{
    public class EmailService(IOptions<MailOptions> emailOptions) : IEmailService
    {
        private readonly MailOptions _emailOptions = emailOptions.Value;

        public async Task SendEmailAsync(string to, string subject, string body) =>
            await SendViaSmtpClient(CreateMessage(to, subject, body));

        public async Task SendEmailsAsync(string subject, IEnumerable<(string to, string body)> infos)
        {
            List<MimeMessage> messages = [];

            foreach (var (to, body) in infos)
                messages.Add(CreateMessage(to, subject, body));

            await SendViaSmtpClient([.. messages]);
        }

        private async Task SendViaSmtpClient(params MimeMessage[] messages)
        {
#if DEBUG
            return;
#endif
            using var client = new SmtpClient() { Timeout = 10000 };

            try
            {
                await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, useSsl: true);
                await client.AuthenticateAsync(_emailOptions.SmtpUser, _emailOptions.SmtpPassword);

                foreach (var message in messages)
                    try { await client.SendAsync(message); }
                    catch { continue; }
            }
            catch { }
            finally { await client.DisconnectAsync(true); }
        }

        private MimeMessage CreateMessage(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SmtpUser));
            emailMessage.To.Add(new MailboxAddress("Отримувач", to));
            emailMessage.Subject = subject;

            var bodyPart = new TextPart("plain") { Text = body };

            emailMessage.Body = bodyPart;

            return emailMessage;
        }
    }
}
