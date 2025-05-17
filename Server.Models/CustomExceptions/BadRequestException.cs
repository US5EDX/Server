namespace Server.Models.CustomExceptions;

public class BadRequestException(string message, string[]? errors = null) : Exception(message)
{
    public string[] Errors { get; } = errors ?? [];
}
