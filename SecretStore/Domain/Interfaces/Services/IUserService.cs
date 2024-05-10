using SecretStore.Domain.Models;

namespace SecretStore.Domain.Interfaces.Services;

public interface IUserService
{
    Task<Guid> ValidateCredentials(Credentials credentials);
    Task<Guid> Add(string clientId, string clientSecret);
}