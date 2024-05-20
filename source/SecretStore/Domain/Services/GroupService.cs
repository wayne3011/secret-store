using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Domain.Models;

namespace SecretStore.Domain.Services;

public class GroupService : IGroupService
{
    private readonly IGroupsRepository _groupsRepository;
    private readonly ISecretRepository _secretRepository;
    private readonly IUserRepository _userRepository;
    
    public GroupService(IGroupsRepository groupsRepository, ISecretRepository secretRepository, IUserRepository userRepository)
    {
        _groupsRepository = groupsRepository;
        _secretRepository = secretRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Create(string name)
    {
        var group = await _groupsRepository.Get(name);
        if (group is not null)
        {
            throw new InvalidGroupException("Group with this name already exist.");
        }
        
        var id = await _groupsRepository.Add(new Group()
        {
            Name = name,
            Secrets = new List<Secret>(),
            Users = new List<User>()
        });

        return id;
    }

    public async Task AddUser(string groupName, string clientId)
    {
        var group = await _groupsRepository.Get(groupName);
        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }

        var user = await _userRepository.Get(clientId);
        
        if(user is null)
        {
            throw new InvalidClientIdException("Invalid client id.");
        }
        
        await _groupsRepository.AddUser(group.Id, user.Id);
    }

    public async Task<Secret> AddSecret(string groupName, string name, string value)
    {
        var group = await _groupsRepository.Get(groupName);
        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }
        var secret = new Secret
        {
            GroupId = group.Id,
            Name = name,
            Value = value,
            Occupied = false
        };
        var secretId = await _secretRepository.Add(secret);
        secret.Id = secretId;
        return secret;
    }

    public async Task<bool> CheckHaveAccess(Guid userId, string groupName)
    {
        var user = await _userRepository.Get(userId);

        if (user is null)
        {
            throw new InvalidUserIdException("Invalid user id.");
        }

        return user.Groups.Any(x => x.Name == groupName);
    }
    
    public async Task DeleteSecret(string groupName, string secretName)
    {
        var group = await _groupsRepository.Get(groupName);

        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }
        
        var secret = await _secretRepository.Get(group.Id, secretName);

        if (secret is null)
        {
            throw new InvalidSecretException("Invalid secret name.");
        }

        await _secretRepository.Delete(secret.Id);
    }
}