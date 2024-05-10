namespace SecretStore.Web.Controllers.DTOs;

public class AuthDto
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}