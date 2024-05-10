namespace SecretStore.Web.Options;

public class AdminAccessConfiguration
{
    public const string SectionName = "AdminAccessConfiguration";
    public required string AdminAccessUrl { get; set; }
    public required string AdminApiKey { get; set; }
    public required string AdminAuthHeader { get; set; }
}