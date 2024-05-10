namespace SecretStore.Domain.Models;

public class Credentials
{
    public required string ClientId { get; set; }
    public required string Secret { get; set; }
}