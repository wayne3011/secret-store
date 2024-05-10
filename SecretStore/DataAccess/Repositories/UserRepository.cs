using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretStore.DataAccess.Context;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly StoreDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(StoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> Get(string clientId)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(x => x.ClientId == clientId);
        return _mapper.Map<User>(userDb);
    }

    public async Task<User?> Get(Guid userId)
    {
        var userDb = await _context.Users.Include(x => x.Groups).FirstOrDefaultAsync(x => x.Id == userId);
        return _mapper.Map<User>(userDb);
    }

    public async Task<Guid> Add(User user)
    {
        var userDb = _mapper.Map<UserDb>(user);

        var newUser = await _context.Users.AddAsync(userDb);

        await _context.SaveChangesAsync();

        return newUser.Entity.Id;
    }
}