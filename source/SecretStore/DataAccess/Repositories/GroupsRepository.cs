using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretStore.DataAccess.Context;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Repositories;

public class GroupsRepository : IGroupsRepository
{
    private readonly StoreDbContext _context;
    private readonly IMapper _mapper;
    
    public GroupsRepository(StoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Add(Group group)
    {
        var groupDb = _mapper.Map<GroupDb>(group);
        var newGroup = await _context.AddAsync(groupDb);

        await _context.SaveChangesAsync();

        return newGroup.Entity.Id;
    }

    public async Task<Group?> Get(string groupName)
    {
        var groupDb = await _context.Groups.Include(x => x.Users).FirstOrDefaultAsync(x => x.Name == groupName);
        return _mapper.Map<Group>(groupDb);
    }

    public async Task AddUser(Guid groupId, Guid userId)
    {
        var group = await _context.Groups.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == groupId);
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        
        group.Users.Add(user);
        
        await _context.SaveChangesAsync();
    }
}