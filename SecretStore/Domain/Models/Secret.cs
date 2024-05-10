namespace SecretStore.Domain.Models;

public class Secret
{
    public Guid Id { get; set; }
    public required Guid GroupId { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
    public required bool Occupied { get; set; }
    public Guid? OccupierId { get; set; }
}
    