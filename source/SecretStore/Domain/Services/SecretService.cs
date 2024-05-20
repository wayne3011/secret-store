using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Domain.Models;

namespace SecretStore.Domain.Services;

public class SecretService : ISecretService
{
    private readonly ISecretRepository _secretRepository;
    private readonly IGroupsRepository _groupsRepository;

    public SecretService(ISecretRepository secretRepository, IGroupsRepository groupsRepository)
    {
        _secretRepository = secretRepository;
        _groupsRepository = groupsRepository;
    }

    public async Task<Secret?> Get(string name, string groupName)
    {
        var group = await _groupsRepository.Get(groupName);

        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }

        var secret = await _secretRepository.Get(group.Id, name);

        return secret;
    }

    public async Task Occupy(string name, string groupName, Guid userId)
    {
        var group = await _groupsRepository.Get(groupName);

        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }
        
        var secret = await _secretRepository.Get(group.Id, name);

        if (secret is null)
        {
            throw new InvalidSecretException("Invalid secret name.");
        }
        
        if (secret.Occupied)
        {
            throw new SecretAlreadyOccupiedException("Secret already occupied.");
        }

        await _secretRepository.UpdateStatus(secret.Id, true, userId);
    }

    public async Task Release(string name, string groupName, Guid userId)
    {
        var group = await _groupsRepository.Get(groupName);

        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }
        
        var secret = await _secretRepository.Get(group.Id, name);

        if (secret is null)
        {
            throw new InvalidSecretException("Invalid secret name.");
        }

        if (secret.OccupierId != userId)
        {
            throw new SecretPermissionDeniedException("The secret can only be unlocked by the user who has taken the lock.");
        }
        
        await _secretRepository.UpdateStatus(secret.Id, false, userId);
    }

    public async Task<bool> CheckBusy(string name, string groupName)
    {
        var group = await _groupsRepository.Get(groupName);

        if (group is null)
        {
            throw new InvalidGroupException("Invalid group name.");
        }
        
        var secret = await _secretRepository.Get(group.Id, name);

        if (secret is null)
        {
            throw new InvalidSecretException("Invalid secret name.");
        }

        return secret.Occupied;
    }
}