namespace Server.Models.CustomExceptions;

public class UnauthorizedException(string message) : Exception(message);
