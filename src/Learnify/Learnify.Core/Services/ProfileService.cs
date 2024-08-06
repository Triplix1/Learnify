using AutoMapper;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.WebUtilities;

namespace Learnify.Core.Services;

/// <inheritdoc />
public class ProfileService : IProfileService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobStorage _blobStorage;

    /// <summary>
    /// Initializes new instance of <see cref="ProfileService"/>
    /// </summary>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="blobStorage"><see cref="IBlobStorage"/></param>
    /// <param name="psqUnitOfWork"><see cref="IPsqUnitOfWork"/></param>
    public ProfileService(IMapper mapper, IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage)
    {
        _mapper = mapper;
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProfileResponse>> GetByIdAsync(int id)
    {
        var profile = await _psqUnitOfWork.UserRepository.GetByIdAsync(id);

        return ApiResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(profile));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<ProfileResponse>>> GetAllProfilesAsync()
    {
        var profiles = await _psqUnitOfWork.UserRepository.GetFilteredAsync();

        return ApiResponse<IEnumerable<ProfileResponse>>.Success(_mapper.Map<IEnumerable<ProfileResponse>>(profiles));
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var user = await _psqUnitOfWork.UserRepository.GetByIdAsync(id);
        
        if(user is null)
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find user with such id"));
        
        if (!await _psqUnitOfWork.UserRepository.DeleteAsync(user.Id))
            return ApiResponse.Failure(new KeyNotFoundException("Cannot find user with such id"));

        await _psqUnitOfWork.SaveChangesAsync();

        if (user.ImageContainerName is not null && user.ImageBlobName is not null)
        {
            await _blobStorage.DeleteAsync(user.ImageContainerName, user.ImageBlobName);
        }

        return ApiResponse.Success();
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProfileResponse>> UpdateAsync(ProfileUpdateRequest profileUpdateRequest)
    {
        var origin = await _psqUnitOfWork.UserRepository.GetByIdAsync(profileUpdateRequest.Id);

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
            
            // var buffer = new byte[10000];
            // var bytesRead = 0;
            var fileBytes = Array.Empty<byte>();
            // var totalBytesRead = 0;

            // using var memoryStream = new MemoryStream();
            // await profileUpdateRequest.File.CopyToAsync(memoryStream);
            // fileBytes = memoryStream.ToArray();

            await using var stream = profileUpdateRequest.File.OpenReadStream();

            byte[] b;

            using (BinaryReader br = new BinaryReader(stream))
            {
                b = br.ReadBytes((int)stream.Length);
            }
            
            var blobDto = new BlobDto()
            {
                Name = origin.ImageBlobName,
                ContainerName = origin.ImageContainerName,
                Content = b
            };
            
            var photoResult = await _blobStorage.UploadAsync(blobDto);

            origin.ImageUrl = photoResult.Url;
            origin.ImageContainerName = photoResult.ContainerName;
            origin.ImageBlobName = photoResult.Name;
        }

        await _psqUnitOfWork.UserRepository.UpdateAsync(origin);
        await _psqUnitOfWork.SaveChangesAsync();
        
        return ApiResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(origin));
    }
}