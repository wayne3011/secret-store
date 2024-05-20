using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Repositories;

public interface ITokensRepository
{
    Task<Guid> Add(Guid userId, string refreshToken);
    Task<Guid> Update(Guid userId, string refreshToken);
    Task<Tokens?> Get(string refreshToken);
    Task<Tokens?> Get(Guid userId);
    Task Delete(Guid tokenId);
}