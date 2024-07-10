using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Enums;
using AutoMapper;
using Contracts;

namespace AuthIdentity.Core.MappingProfile;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserCreated>()
            .ForMember(uc => uc.Id, u => u.MapFrom(us => us.Id.ToString()))
            .ForMember(uc => uc.Role, u => u.MapFrom(us => us.Role.ToString()));
        CreateMap<UserUpdated, User>()
            .ForMember(uc => uc.Id, u => u.MapFrom(us => us.Id.ToString()))
            .ForMember(uc => uc.Role, u => u.MapFrom(us => Enum.Parse<Role>(us.Role)));
        CreateMap<RoleRequest, Role>()
            .ConvertUsing(src => Enum.Parse<Role>(src.ToString()));
    }
}