using AutoMapper;
using BlobStorage.Grpc.Protos;
using Contracts.Blob;
using General.Dto;
using Google.Protobuf;
using MassTransit;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Core.DTO;
using Profile.Core.ServiceContracts;

namespace Profile.Core.Services;

/// <inheritdoc />
public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobStorageGrpcService _blobStorageGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>
    /// Initializes new instance of <see cref="ProfileService"/>
    /// </summary>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="blobStorageGrpcService"><see cref="IBlobStorageGrpcService"/></param>
    /// <param name="publishEndpoint"><see cref="IPublishEndpoint"/></param>
    /// <param name="unitOfWork"><see cref="IUnitOfWork"/></param>
    public ProfileService(IMapper mapper, IBlobStorageGrpcService blobStorageGrpcService, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _blobStorageGrpcService = blobStorageGrpcService;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProfileResponse>> GetByIdAsync(string id)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(id);

        return ApiResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(profile));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<ProfileResponse>>> GetAllProfilesAsync()
    {
        var profiles = await _unitOfWork.ProfileRepository.GetAllAsync();

        return ApiResponse<IEnumerable<ProfileResponse>>.Success(_mapper.Map<IEnumerable<ProfileResponse>>(profiles));
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(string id)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(id);
        
        if(profile is null)
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find user with such id"));
        
        if (!await _unitOfWork.ProfileRepository.DeleteAsync(profile))
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find user with such id"));

        await _unitOfWork.SaveChangesAsync();

        if (profile.ImageContainerName is not null && profile.ImageBlobName is not null)
        {
            var deletedBlob = new BlobDeleteRequest()
            {
                Name = profile.ImageBlobName,
                ContainerName = profile.ImageContainerName
            };

            await _publishEndpoint.Publish(deletedBlob);
        }

        return ApiResponse.Success();
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProfileResponse>> UpdateAsync(ProfileUpdateRequest profileUpdateRequest)
    {
        var origin = await _unitOfWork.ProfileRepository.GetByIdAsync(profileUpdateRequest.Id);

        if (origin is null)
            ApiResponse<ProfileResponse>.Failure(new KeyNotFoundException("Cannot find user with such id"));
        
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

        await _unitOfWork.ProfileRepository.UpdateAsync(origin);
        await _unitOfWork.SaveChangesAsync();
        
        return ApiResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(origin));
    }
}