namespace SecretStore.DataAccess.Models;

public record GroupDb
{
    public required string Name { get;set; }
    public Guid Id { get;set; }
    public required ICollection<SecretDb> Secrets { get;set; } 
    public required ICollection<UserDb> Users { get;set;}
    
}
