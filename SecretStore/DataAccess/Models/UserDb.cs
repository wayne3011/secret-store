namespace SecretStore.DataAccess.Models;

public record UserDb
{
    public Guid Id { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required ICollection<GroupDb> Groups { get; set; }
    public ICollection<TokensDb> Tokens { get; set; }
}