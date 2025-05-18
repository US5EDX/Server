namespace Server.Services.Options.SettingsOptions;

public class MailOptions
{
    public string SenderName { get; set; } = default!;
    public string SmtpServer { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; } = default!;
    public string SmtpPassword { get; set; } = default!;
}
