using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Services;

public interface ISecretService
{
    Task<Secret?> Get(string name, string groupName);
    Task Occupy(string name, string groupName, Guid userId);
    Task Release(string name, string groupName, Guid userId);
    Task<bool> CheckBusy(string name, string groupName);
}