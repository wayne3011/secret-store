using AutoMapper;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDb, User>().ReverseMap();
    }
}