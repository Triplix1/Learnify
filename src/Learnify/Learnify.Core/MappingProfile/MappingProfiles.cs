using AutoMapper;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Enums;

namespace Learnify.Core.MappingProfile;

public class MappingProfiles: Profile
{
    public MappingProfiles() : base()
    {
        // Role
        CreateMap<RoleRequest, Role>()
            .ConvertUsing(src => Enum.Parse<Role>(src.ToString()));

        // User
        CreateMap<User, ProfileResponse>();
        CreateMap<ProfileUpdateRequest, User>();
    }
}