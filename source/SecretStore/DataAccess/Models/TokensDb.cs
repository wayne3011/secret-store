namespace SecretStore.DataAccess.Models;

public record TokensDb
{
    public required string RefreshToken { get; set; }
    public Guid Id { set; get; }
    public required Guid UserId { get; set; }
    public UserDb User { get; set; }
}