using AutoMapper;
using Contracts;
using IdentityService.Models;

namespace IdentityService.MapperProfile;

public class UserMapperProfile : Profile
{
    UserMapperProfile()
    {
        CreateMap<ApplicationUser, UserCreated>()
            .ForMember(u => u.Type, u => u.MapFrom(us => us.Type.ToString()));
    }
}