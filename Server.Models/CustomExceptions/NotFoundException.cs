namespace Server.Models.CustomExceptions;

public class NotFoundException(string message) : Exception(message);

