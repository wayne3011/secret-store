namespace SecretStore.Web.Controllers.DTOs;

public class PushSecretDto
{
    public required string Name { get; set; }
    public required string Value { get; set; }
}