using AutoMapper;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Mapper;

public class SecretProfile : Profile
{
    public SecretProfile()
    {
        CreateMap<Secret, SecretDb>().ReverseMap();
    }
}