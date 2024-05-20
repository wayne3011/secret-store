using AutoMapper;
using SecretStore.DataAccess.Models;
using SecretStore.Domain.Models;

namespace SecretStore.DataAccess.Mapper;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDb>().ReverseMap();
    }
}