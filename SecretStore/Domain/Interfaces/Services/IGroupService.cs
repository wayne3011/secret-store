using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Services;

public interface IGroupService
{
    Task<Guid> Create(string name);
    Task AddUser(string groupName, string clientId);
    Task<Secret> AddSecret(string groupName, string name, string value);
    Task<bool> CheckHaveAccess(Guid userId, string groupName);
}