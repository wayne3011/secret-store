using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Repositories;

public interface IGroupsRepository
{
    Task<Guid> Add(Group group);
    Task<Group?> Get(string groupName);
    Task AddUser(Guid groupId, Guid userId);
}