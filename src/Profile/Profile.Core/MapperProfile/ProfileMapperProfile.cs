using Contracts;
using Contracts.User;
using Profile.Core.Domain.Entities;
using Profile.Core.DTO;

namespace Profile.Core.MapperProfile;

/// <inheritdoc />
public class ProfileMapperProfile : AutoMapper.Profile
{
    /// <inheritdoc />
    public ProfileMapperProfile()
    {
        CreateMap<ProfileUpdateRequest, User>();
        CreateMap<User, ProfileResponse>();
        CreateMap<UserCreated, User>()
            .ForMember(u => u.Type, u => u.MapFrom(uc => ConvertToEnum(uc.Role)));
        CreateMap<ProfileResponse, UserUpdated>();
    }

    private static UserType ConvertToEnum(string value)
    {
        Enum.TryParse(value, true, out UserType result);
        return result;
    }
}