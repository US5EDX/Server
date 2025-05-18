namespace Server.Services.Options.ContextOptions.RequestContext
{
    public interface IRequestContext
    {
        string? IpAddress { get; set; }
        string? UserId { get; set; }
    }
}