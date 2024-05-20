namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class SecretAlreadyOccupiedException : Exception
{
    public SecretAlreadyOccupiedException(string? message) : base(message)
    {
    }

    public SecretAlreadyOccupiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}