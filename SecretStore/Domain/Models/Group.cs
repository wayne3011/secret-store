namespace SecretStore.Domain.Models;

public class Group
{
    public required string Name { get;set; }
    public Guid Id { get;set; }
    public required List<Secret> Secrets { get;set; } 
    public required List<User> Users { get;set;}
}