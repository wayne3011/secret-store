using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> Get(string clientId);
    Task<User?> Get(Guid userId);
    Task<Guid> Add(User user);
}