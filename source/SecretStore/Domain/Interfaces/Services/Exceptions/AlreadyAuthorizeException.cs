namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class AlreadyAuthorizeException : Exception
{
    public AlreadyAuthorizeException(string? message) : base(message)
    {
    }

    public AlreadyAuthorizeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}