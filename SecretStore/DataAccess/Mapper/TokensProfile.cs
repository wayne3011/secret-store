using AutoMapper;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Mapper;

public class TokensProfile : Profile
{
    public TokensProfile()
    {
        CreateMap<Tokens, TokensDb>().ReverseMap();
    }
}