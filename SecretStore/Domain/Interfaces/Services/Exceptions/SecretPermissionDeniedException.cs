namespace SecretStore.Domain.Interfaces.Services.Exceptions;

public class SecretPermissionDeniedException : Exception
{
    public SecretPermissionDeniedException(string? message) : base(message)
    {
    }

    public SecretPermissionDeniedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}