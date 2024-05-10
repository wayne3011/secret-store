namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class InvalidSecretException : Exception
{
    public InvalidSecretException(string? message) : base(message)
    {
    }

    public InvalidSecretException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}