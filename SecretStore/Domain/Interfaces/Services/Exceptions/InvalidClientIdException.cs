namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class InvalidClientIdException : Exception
{
    public InvalidClientIdException(string? message) : base(message)
    {
    }

    public InvalidClientIdException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}