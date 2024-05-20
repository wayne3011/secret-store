namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException(string? message) : base(message)
    {
    }

    public InvalidRefreshTokenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}