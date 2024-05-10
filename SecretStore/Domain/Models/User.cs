namespace SecretStore.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required List<Group> Groups { get; set; }
    public required List<Tokens> Tokens { get; set; } 
}