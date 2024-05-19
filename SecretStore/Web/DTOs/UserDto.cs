using SecretStore.Domain.Models;

namespace SecretStore.Web.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public required string ClientId { get; set; }
    public required List<Group> Groups { get; set; }
    public required List<Tokens> Tokens { get; set; }
}