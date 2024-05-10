namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class InvalidGroupException : Exception
{
    public InvalidGroupException(string? message) : base(message)
    {
    }

    public InvalidGroupException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}