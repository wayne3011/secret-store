using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretStore.DataAccess.Context;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Repositories;

public class SecretRepository : ISecretRepository
{
    private readonly IMapper _mapper;
    private readonly StoreDbContext _context;

    public SecretRepository(IMapper mapper, StoreDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Guid> Add(Secret value)
    {
        var secretDb = _mapper.Map<SecretDb>(value);

        var newSecret = await _context.AddAsync(secretDb);

        await _context.SaveChangesAsync();

        return newSecret.Entity.Id;
    }

    public async Task<Secret?> Get(Guid groupId, string name)
    {
        var secretDb = await _context.Secrets.FirstOrDefaultAsync(x => x.GroupId == groupId && x.Name == name);
        return _mapper.Map<Secret>(secretDb);
    }

    public async Task UpdateStatus(Guid id, bool occupied, Guid userId)
    {
        var secretDb = await _context.Secrets.FirstOrDefaultAsync(x => x.Id == id);
        secretDb.Occupied = occupied;
        
        if (occupied)
        {
            secretDb.OccupierId = userId;
        }
        else
        {
            secretDb.OccupierId = null;
        }

        await _context.SaveChangesAsync();
    }
}