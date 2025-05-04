using System.Security.Authentication;
using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.Params;
using Learnify.Core.Dto.Profile;
using Learnify.Core.Enums;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification.Course;
using Learnify.Core.Specification.Filters;
using Learnify.Core.Specification.Profile;
using Learnify.Core.Transactions;

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
        var profile = await _psqUnitOfWork.UserRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        var profileResponse = _mapper.Map<ProfileResponse>(profile);

        return profileResponse;
    }

    public async Task<PagedList<ProfileResponse>> GetModeratorsAsync(ModeratorsListParams moderatorsListParams,
        CancellationToken cancellationToken = default)
    {
        var filter = new EfFilter<User>()
        {
            Specification = new RolesSpecification(Role.Moderator),
            OrderByParams = moderatorsListParams.OrderByParams,
            PagedListParams = moderatorsListParams.PagedListParams
        };

        var pagedResult = await _psqUnitOfWork.UserRepository.GetPagedAsync(filter, cancellationToken);

        var profileResponses = _mapper.Map<IEnumerable<ProfileResponse>>(pagedResult.Items);

        return new PagedList<ProfileResponse>(profileResponses, pagedResult.TotalCount, pagedResult.CurrentPage,
            pagedResult.PageSize);
    }

    public async Task<PagedList<ProfileResponse>> GetAdminsAsync(AdminsListParams adminsListParams, CancellationToken cancellationToken = default)
    {
        var filter = new EfFilter<User>()
        {
            Specification = new RolesSpecification(Role.Admin),
            OrderByParams = adminsListParams.OrderByParams,
            PagedListParams = adminsListParams.PagedListParams
        };

        var pagedResult = await _psqUnitOfWork.UserRepository.GetPagedAsync(filter, cancellationToken);

        var profileResponses = _mapper.Map<IEnumerable<ProfileResponse>>(pagedResult.Items);

        return new PagedList<ProfileResponse>(profileResponses, pagedResult.TotalCount, pagedResult.CurrentPage,
            pagedResult.PageSize);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _psqUnitOfWork.UserRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("Cannot find user with such id");

        var deletionResult = await _psqUnitOfWork.UserRepository.DeleteAsync(user.Id, cancellationToken);

        if (!deletionResult)
            throw new KeyNotFoundException("Cannot find user with such id");
    }

    /// <inheritdoc />
    public async Task<ProfileResponse> UpdateAsync(ProfileUpdateRequest profileUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var origin =
            await _psqUnitOfWork.UserRepository.GetByIdAsync(profileUpdateRequest.Id,
                cancellationToken: cancellationToken);

        if (origin is null)
            throw new KeyNotFoundException("Cannot find user with such id");

        _mapper.Map(profileUpdateRequest, origin);
        // using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        // {
            // if (profileUpdateRequest.File is not null)
            // {
            //     if (origin.ImageContainerName is not null && origin.ImageBlobName is not null)
            //     {
            //         await _blobStorage.DeleteAsync(origin.ImageContainerName, origin.ImageBlobName, cancellationToken);
            //     }
            //     else
            //     {
            //         origin.ImageContainerName = "profile";
            //         origin.ImageBlobName = profileUpdateRequest.File.FileName;
            //     }
            //
            //     await using var stream = profileUpdateRequest.File.OpenReadStream();
            //
            //     var blobDto = new BlobDto()
            //     {
            //         Name = origin.ImageBlobName,
            //         ContainerName = origin.ImageContainerName,
            //         Content = stream,
            //         ContentType = "image/*"
            //     };
            //
            //     var photoResult = await _blobStorage.UploadAsync(blobDto, cancellationToken: cancellationToken);
            //
            //     origin.ImageUrl = photoResult.Url;
            //     origin.ImageContainerName = photoResult.ContainerName;
            //     origin.ImageBlobName = photoResult.Name;
            // }

            await _psqUnitOfWork.UserRepository.UpdateAsync(origin, cancellationToken);

        //     ts.Complete();
        // }

        var profileResponse = _mapper.Map<ProfileResponse>(origin);

        return profileResponse;
    }

    public async Task<ProfileResponse> UpdateRoleAsync(int userId, UpdateUserRoleRequest updateUserRoleRequest,
        CancellationToken cancellationToken = default)
    {
        var user = await _psqUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken: cancellationToken);

        if (user == null)
            throw new KeyNotFoundException("Cannot find user with such id");

        var roleToSet = _mapper.Map<Role>(updateUserRoleRequest.Role);

        if (user.Role == roleToSet)
            return _mapper.Map<ProfileResponse>(user);

        if (roleToSet == Role.Student)
            await ValidatePossibilityToBecomeStudent(cancellationToken, user);

        user.Role = roleToSet;

        user = await _psqUnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

        var profileResponse = _mapper.Map<ProfileResponse>(user);

        return profileResponse;
    }

    private async Task ValidatePossibilityToBecomeStudent(CancellationToken cancellationToken, User user)
    {
        var specification = new CourseAuthorSpecification(user.Id);

        var filter = new EfFilter<Course>()
        {
            Specification = specification,
        };
        var userCourses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter, cancellationToken);

        if (userCourses.TotalCount > 0)
            throw new InvalidOperationException("You cannot update the profile of a student");
    }
}