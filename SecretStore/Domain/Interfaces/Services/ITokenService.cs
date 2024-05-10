using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<Tokens> IssueTokens(Guid userId);
    Task<Tokens> ReissueTokens(string refreshToken); 
}