namespace SecretStore.Web.DTOs;

public class TokensDto
{
    public required Guid UserId { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}