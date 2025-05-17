namespace Server.Models.CustomExceptions;

public sealed class CustomInvalidCastException(string message) : BadRequestException(message);