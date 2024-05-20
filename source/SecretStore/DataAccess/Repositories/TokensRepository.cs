using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretStore.DataAccess.Context;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Repositories;

public class TokensRepository : ITokensRepository
{
    private readonly StoreDbContext _context;
    private readonly IMapper _mapper;

    public TokensRepository(StoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Add(Guid userId, string refreshToken)
    {
        var token = await _context.Tokens.AddAsync(new TokensDb
        {
            RefreshToken = refreshToken,
            UserId = userId
        });

        await _context.SaveChangesAsync();

        return token.Entity.Id;
    }

    public async Task<Guid> Update(Guid userId, string refreshToken)
    {
        var token = await _context.Tokens.FirstOrDefaultAsync(x => x.UserId == userId);

        token.RefreshToken = refreshToken;

        await _context.SaveChangesAsync();
        return token.Id;
    }

    public async Task<Tokens?> Get(string refreshToken)
    {
        var tokens = await _context.Tokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        return _mapper.Map<Tokens>(tokens);
    }

    public async Task<Tokens?> Get(Guid userId)
    {
        var tokens = await _context.Tokens.FirstOrDefaultAsync(x => x.UserId == userId);
        return _mapper.Map<Tokens>(tokens);
    }

    public async Task Delete(Guid tokenId)
    {
        await _context.Tokens.Where(x => x.Id == tokenId).ExecuteDeleteAsync();
    }
}