using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Domain.Models;

namespace SecretStore.Domain.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Guid> ValidateCredentials(Credentials credentials)
    {
        var user = await userRepository.Get(credentials.ClientId);

        if (user is null)
        {
            throw new InvalidCredentialException("Invalid credentials.");
        }

        var isValid = BCrypt.Net.BCrypt.Verify(text: credentials.Secret, hash: user.ClientSecret);
        if (!isValid)
        {
            throw new InvalidCredentialException("Invalid credentials.");
        }    

        return user.Id;
    }

    public async Task<Guid> Add(string clientId, string clientSecret)
    {
        var user = await userRepository.Get(clientId);
        if (user is not null)
        {
            throw new ClientIdOccupiedException("User with this client id already exist.");
        }
        
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var clientSecretHash = BCrypt.Net.BCrypt.HashPassword(clientSecret, salt);
        
        var guid = await userRepository.Add(new User
        {
            ClientId = clientId,
            ClientSecret = clientSecretHash,
            Groups = new List<Group>(),
            Tokens = new List<Tokens>(),
        });
        
        return guid;
    }
}