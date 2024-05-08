using System.Text.RegularExpressions;

namespace SecretStore.DataAccess.Models;

public record SecretDb
{
    public Guid Id { get; set; }
    public required Guid GroupId { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
    public bool Occupied { get; set; }
    public GroupDb Group { get; set; } 
}