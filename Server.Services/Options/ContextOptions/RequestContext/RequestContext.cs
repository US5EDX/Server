namespace Server.Services.Options.ContextOptions.RequestContext;

public class RequestContext : IRequestContext
{
    public string? UserId { get; set; }
    public string? IpAddress { get; set; }
}
