using AutoMapper;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

/// <inheritdoc />
public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobStorage _blobStorage;

    /// <summary>
    /// Initializes new instance of <see cref="ProfileService"/>
    /// </summary>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="blobStorage"><see cref="IBlobStorage"/></param>
    /// <param name="unitOfWork"><see cref="IUnitOfWork"/></param>
    public ProfileService(IMapper mapper, IUnitOfWork unitOfWork, IBlobStorage blobStorage)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _blobStorage = blobStorage;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProfileResponse>> GetByIdAsync(int id)
    {
        var profile = await _unitOfWork.UserRepository.GetByIdAsync(id);

        return ApiResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(profile));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<ProfileResponse>>> GetAllProfilesAsync()
    {
        var profiles = await _unitOfWork.UserRepository.GetAllAsync();

        return ApiResponse<IEnumerable<ProfileResponse>>.Success(_mapper.Map<IEnumerable<ProfileResponse>>(profiles));
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        
        if(user is null)
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find user with such id"));
        
        if (!await _unitOfWork.UserRepository.DeleteAsync(user.Id))
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find user with such id"));

        await _unitOfWork.SaveChangesAsync();

        if (user.ImageContainerName is not null && user.ImageBlobName is not null)
        {
            await _blobStorage.DeleteAsync(user.ImageContainerName, user.ImageBlobName);
        }

        return ApiResponse.Success();
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProfileResponse>> UpdateAsync(ProfileUpdateRequest profileUpdateRequest)
    {
        var origin = await _unitOfWork.UserRepository.GetByIdAsync(profileUpdateRequest.Id);

        if (origin is null)
            return ApiResponse<ProfileResponse>.Failure(new KeyNotFoundException("Cannot find user with such id"));
        
        _mapper.Map(profileUpdateRequest, origin);

        if (profileUpdateRequest.File is not null)
        {
            if (origin.ImageContainerName is not null && origin.ImageBlobName is not null)
            {
                await _blobStorage.DeleteAsync(origin.ImageContainerName, origin.ImageBlobName);
            }
            else
            {
                origin.ImageContainerName = "profile";
                origin.ImageBlobName = profileUpdateRequest.File.FileName;
            }

            using var memoryStream = new MemoryStream();
            await profileUpdateRequest.File.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            
            var blobDto = new BlobDto()
            {
                Name = origin.ImageBlobName,
                ContainerName = origin.ImageContainerName,
                Content = fileBytes
            };
            
            var photoResult = await _blobStorage.UploadAsync(blobDto);

            origin.ImageUrl = photoResult.Url;
            origin.ImageContainerName = photoResult.ContainerName;
            origin.ImageBlobName = photoResult.Name;
        }

        await _unitOfWork.UserRepository.UpdateAsync(origin);
        await _unitOfWork.SaveChangesAsync();
        
        return ApiResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(origin));
    }
}