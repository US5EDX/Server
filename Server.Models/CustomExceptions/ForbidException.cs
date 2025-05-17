namespace Server.Models.CustomExceptions;

public class ForbidException(string message) : Exception(message);