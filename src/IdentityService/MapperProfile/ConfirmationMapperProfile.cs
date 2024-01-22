using AutoMapper;
using IdentityService.DTOs;
using IdentityService.Models;

namespace IdentityService.MapperProfile;

public class ConfirmationMapperProfile : Profile
{
    public ConfirmationMapperProfile()
    {
        CreateMap<Confirmation, ConfirmationResponse>();
        CreateMap<ConfirmationAddRequest, Confirmation>();
    }
}