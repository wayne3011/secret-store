namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class InvalidUserIdException : Exception
{
    public InvalidUserIdException(string? message) : base(message)
    {
    }

    public InvalidUserIdException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}