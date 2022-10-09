using AlpaTabApi.Dtos;
using AlpaTabApi.Models;
using AutoMapper;

namespace AlpaTabApi.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Source -> Target
        CreateMap<AlpaTabUser, UserReadDto>();
        CreateMap<UserWriteDto, AlpaTabUser>();
    }
}
