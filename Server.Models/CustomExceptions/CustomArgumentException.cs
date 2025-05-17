namespace Server.Models.CustomExceptions;

public sealed class CustomArgumentException(string message) : BadRequestException(message);