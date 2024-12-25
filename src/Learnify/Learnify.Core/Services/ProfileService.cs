using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.Params;
using Learnify.Core.Dto.Profile;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification;
using Learnify.Core.Transactions;
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
    public async Task<ProfileResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var profile = await _psqUnitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

        var profileResponse = _mapper.Map<ProfileResponse>(profile);

        return profileResponse;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _psqUnitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("Cannot find user with such id");

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            var deletionResult = await _psqUnitOfWork.UserRepository.DeleteAsync(user.Id, cancellationToken);

            if (!deletionResult)
                throw new KeyNotFoundException("Cannot find user with such id");

            if (user.ImageContainerName is not null && user.ImageBlobName is not null)
                await _blobStorage.DeleteAsync(user.ImageContainerName, user.ImageBlobName, cancellationToken);

            transaction.Complete();
        }
    }

    /// <inheritdoc />
    public async Task<ProfileResponse> UpdateAsync(ProfileUpdateRequest profileUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var origin = await _psqUnitOfWork.UserRepository.GetByIdAsync(profileUpdateRequest.Id, cancellationToken);

        if (origin is null)
            throw new KeyNotFoundException("Cannot find user with such id");

        _mapper.Map(profileUpdateRequest, origin);
        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            if (profileUpdateRequest.File is not null)
            {
                if (origin.ImageContainerName is not null && origin.ImageBlobName is not null)
                {
                    await _blobStorage.DeleteAsync(origin.ImageContainerName, origin.ImageBlobName, cancellationToken);
                }
                else
                {
                    origin.ImageContainerName = "profile";
                    origin.ImageBlobName = profileUpdateRequest.File.FileName;
                }

                await using var stream = profileUpdateRequest.File.OpenReadStream();

                var blobDto = new BlobDto()
                {
                    Name = origin.ImageBlobName,
                    ContainerName = origin.ImageContainerName,
                    Content = stream,
                    ContentType = "image/*"
                };

                var photoResult = await _blobStorage.UploadAsync(blobDto, cancellationToken: cancellationToken);

                origin.ImageUrl = photoResult.Url;
                origin.ImageContainerName = photoResult.ContainerName;
                origin.ImageBlobName = photoResult.Name;
            }

            await _psqUnitOfWork.UserRepository.UpdateAsync(origin, cancellationToken);

            ts.Complete();
        }

        var profileResponse = _mapper.Map<ProfileResponse>(origin);

        return profileResponse;
    }
}