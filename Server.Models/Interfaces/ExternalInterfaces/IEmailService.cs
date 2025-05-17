namespace Server.Models.Interfaces.ExternalInterfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailsAsync(string subject, IEnumerable<(string to, string body)> infos);
    }
}