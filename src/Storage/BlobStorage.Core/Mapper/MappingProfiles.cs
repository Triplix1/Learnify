using AutoMapper;
using BlobStorage.Core.Models;
using ClassLibrary1.Blob;

namespace BlobStorage.Core.Mapper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<BlobAddRequest, BlobDto>();
        CreateMap<BlobResponse, BlobCreatedResponse>();
    }
}