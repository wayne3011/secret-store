namespace SecretStore.Domain.Services.Options;

public class JwtConfiguration
{
    public const string SectionName = "JwtConfiguration";
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public TimeSpan AccessLifetime { get; set; }
    public TimeSpan RefreshLifetime { get; set; }
}