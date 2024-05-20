using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<Tokens> Issue(Guid userId);
    Task<Tokens> Reissue(string refreshToken);
    Task Delete(string refreshToken);
}