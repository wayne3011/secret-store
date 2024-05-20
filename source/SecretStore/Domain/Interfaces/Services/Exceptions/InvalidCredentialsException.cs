namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string? message) : base(message)
    {
    }

    public InvalidCredentialsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}