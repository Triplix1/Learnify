using Contracts;
using Profile.Core.Domain.Entities;
using Profile.Core.DTO;

namespace Profile.Core.MapperProfile;

public class ProfileMapperProfile : AutoMapper.Profile
{
    public ProfileMapperProfile()
    {
        CreateMap<ProfileUpdateRequest, User>();
        CreateMap<User, ProfileResponse>();
        // CreateMap<UserCreated, User>()
        //     .ForMember(u => u.Type, u => u.MapFrom(uc => ConvertToEnum(uc.Type)));
        CreateMap<ProfileResponse, UserUpdated>();
    }

    private static UserType ConvertToEnum(string value)
    {
        Enum.TryParse(value, true, out UserType result);
        return result;
    }
}