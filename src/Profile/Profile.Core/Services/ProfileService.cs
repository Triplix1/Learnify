using AutoMapper;
using BlobStorage.Grpc.Protos;
using Contracts.Blob;
using General.CommonServiceContracts;
using Google.Protobuf;
using MassTransit;
using Profile.Core.Domain.Entities;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Core.DTO;
using Profile.Core.ServiceContracts;

namespace Profile.Core.Services;

/// <inheritdoc />
public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;
    private readonly IBlobStorageGrpcService _blobStorageGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>
    /// Initializes new instance of <see cref="ProfileService"/>
    /// </summary>
    /// <param name="profileRepository"><see cref="IProfileRepository"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="blobStorageGrpcService"><see cref="IBlobStorageGrpcService"/></param>
    /// <param name="publishEndpoint"><see cref="IPublishEndpoint"/></param>
    public ProfileService(IProfileRepository profileRepository, IMapper mapper, IBlobStorageGrpcService blobStorageGrpcService, IPublishEndpoint publishEndpoint)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
        _blobStorageGrpcService = blobStorageGrpcService;
        _publishEndpoint = publishEndpoint;
    }

    /// <inheritdoc />
    public async Task<ProfileResponse> GetByIdAsync(string id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);

        return _mapper.Map<ProfileResponse>(profile);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProfileResponse>> GetAllProfilesAsync()
    {
        var profiles = await _profileRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProfileResponse>>(profiles);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        
        if(profile is null)
            throw new KeyNotFoundException("Cannot find user with such id");
        
        if (!await _profileRepository.DeleteAsync(profile))
        {
            throw new KeyNotFoundException("Cannot find user with such id");
        }

        if (profile.ImageContainerName is not null && profile.ImageBlobName is not null)
        {
            var deletedBlob = new BlobDeleteRequest()
            {
                Name = profile.ImageBlobName,
                ContainerName = profile.ImageContainerName
            };

            await _publishEndpoint.Publish(deletedBlob);
        }
            
    }

    /// <inheritdoc />
    public async Task<ProfileResponse> UpdateAsync(ProfileUpdateRequest profileUpdateRequest)
    {
        var origin = await _profileRepository.GetByIdAsync(profileUpdateRequest.Id);

        if (origin is null)
            throw new KeyNotFoundException("Cannot find user with such id");
        
        _mapper.Map(profileUpdateRequest, origin);

        if (profileUpdateRequest.File is not null)
        {
            if (origin.ImageContainerName is not null && origin.ImageBlobName is not null)
            {
                await _blobStorageGrpcService.DeleteAsync(origin.ImageContainerName, origin.ImageBlobName);
            }

            using var memoryStream = new MemoryStream();
            await profileUpdateRequest.File.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            
            var blobDto = new BlobDto()
            {
                Name = origin.ImageBlobName,
                ContainerName = origin.ImageContainerName,
                Content = ByteString.CopyFrom(fileBytes)
            };
            
            var photoResult = await _blobStorageGrpcService.UpdateAsync(blobDto);

            origin.ImageUrl = photoResult.Url;
            origin.ImageContainerName = photoResult.ContainerName;
            origin.ImageBlobName = photoResult.Name;
        }

        return _mapper.Map<ProfileResponse>(await _profileRepository.UpdateAsync(origin));
    }
}