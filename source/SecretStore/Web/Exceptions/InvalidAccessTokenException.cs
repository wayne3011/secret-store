namespace SecretStore.Web.Exceptions;

public class InvalidAccessTokenException : Exception
{
    public InvalidAccessTokenException(string? message) : base(message)
    {
    }

    public InvalidAccessTokenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}