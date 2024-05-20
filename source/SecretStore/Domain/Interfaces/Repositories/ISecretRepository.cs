using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Repositories;

public interface ISecretRepository
{
    Task<Guid> Add(Secret value);
    Task<Secret?> Get(Guid groupId, string name);
    Task UpdateStatus(Guid id, bool occupied, Guid userId);
    Task Delete(Guid secretId);
}