namespace SecretStore.Domain.Models;

public class Tokens
{
    public Guid Id { set; get; }
    public string? AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required Guid UserId { get; set; }
}