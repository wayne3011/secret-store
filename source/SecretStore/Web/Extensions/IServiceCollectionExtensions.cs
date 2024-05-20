using SecretStore.DataAccess.Repositories;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Models;
using SecretStore.Domain.Services;

namespace SecretStore.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        collection.AddTransient<IGroupService, GroupService>();
        collection.AddTransient<ISecretService, SecretService>();
        collection.AddTransient<ITokenService, TokenService>();
        collection.AddTransient<IUserService, UserService>();
        return collection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection collection)
    {
        collection.AddTransient<IGroupsRepository, GroupsRepository>();
        collection.AddTransient<ISecretRepository, SecretRepository>();
        collection.AddTransient<ITokensRepository, TokensRepository>();
        collection.AddTransient<IUserRepository, UserRepository>();
        return collection;
    }
}