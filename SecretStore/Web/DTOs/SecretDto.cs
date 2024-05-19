namespace SecretStore.Web.DTOs;

public class SecretDto
{
    public Guid Id { get; set; }
    public required string GroupName { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
    public required bool Occupied { get; set; }
    public Guid? OccupierId { get; set; }
}