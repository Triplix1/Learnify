using AutoMapper;
using IdentityService.DTOs.TemporaryUser;
using IdentityService.Models;

namespace IdentityService.MapperProfile;

public class TemporaryUserMapperProfile : Profile
{
    public TemporaryUserMapperProfile()
    {
        CreateMap<TemporaryUserAddRequest, TemporaryUser>();
        CreateMap<TemporaryUser, TemporaryUserResponse>();
    }
}