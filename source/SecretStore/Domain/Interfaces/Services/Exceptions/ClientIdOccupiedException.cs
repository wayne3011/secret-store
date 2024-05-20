namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class ClientIdOccupiedException : Exception
{
    public ClientIdOccupiedException(string? message) : base(message)
    {
    }

    public ClientIdOccupiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}