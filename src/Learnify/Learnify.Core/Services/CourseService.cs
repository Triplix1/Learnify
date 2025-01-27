using System.Linq.Expressions;
using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification.Custom;
using Learnify.Core.Specification.Filters;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class CourseService : ICourseService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IUserValidatorManager _userValidatorManager;
    private readonly IFileService _fileService;
    private readonly IPrivateFileService _privateFileService;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IPsqUnitOfWork psqUnitOfWork, IUserValidatorManager userValidatorManager,
        IFileService fileService, IPrivateFileService privateFileService)
    {
        _mapper = mapper;
        _psqUnitOfWork = psqUnitOfWork;
        _userValidatorManager = userValidatorManager;
        _fileService = fileService;
        _privateFileService = privateFileService;
    }

    public async Task<PagedList<CourseTitleResponse>> GetAllCourseTitles(CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var filter = _mapper.Map<EfFilter<Course>>(courseParams);
        
        if (courseParams.Search is not null)
            filter.Specification &= new SearchCourseSpecification(courseParams.Search);
        
        if (courseParams.AuthorId is not null)
            filter.Specification &= new CourseAuthorSpecification(courseParams.AuthorId.Value);

        filter.Includes = [c => c.Photo];

        var courses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter, cancellationToken);

        var courseTitleResponses = _mapper.Map<IEnumerable<CourseTitleResponse>>(courses.Items);

        var courseTitles = new PagedList<CourseTitleResponse>(courseTitleResponses, courses.TotalCount,
            courses.CurrentPage, courses.PageSize);

        return courseTitles;
    }

    public async Task<PagedList<CourseTitleResponse>> GetMyCourseTitles(int userId, CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        if(userId != courseParams.AuthorId)
            throw new Exception("You have not access to this courses");

        return await GetAllCourseTitles(courseParams, cancellationToken);
    }

    public async Task<IEnumerable<CourseResponse>> GetFilteredAsync(CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var filter = _mapper.Map<EfFilter<Course>>(courseParams);

        if (courseParams.Search is not null)
            filter.Specification = new SearchCourseSpecification(courseParams.Search);

        var courses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter, cancellationToken);
        return _mapper.Map<IEnumerable<CourseResponse>>(courses);
    }

    public async Task<CourseResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var includes = new[] { nameof(Course.Paragraphs), nameof(Course.Video), nameof(Course.Photo) };

        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id, includes, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return courseResponse;
    }

    public async Task<CourseUpdateResponse> PublishAsync(int id, bool publish, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userValidatorManager.ValidateAuthorOfCourseAsync(id, userId, cancellationToken);

        var course = await _psqUnitOfWork.CourseRepository.PublishAsync(id, publish, cancellationToken);

        var courseResponse = _mapper.Map<CourseUpdateResponse>(course);

        return courseResponse;
    }

    public async Task<CourseUpdateResponse> CreateAsync(CourseCreateRequest courseCreateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var course = _mapper.Map<Course>(courseCreateRequest);
        course.AuthorId = userId;
        course.IsPublished = false;

        course = await _psqUnitOfWork.CourseRepository.CreateAsync(course, cancellationToken);

        var courseResponse = _mapper.Map<CourseUpdateResponse>(course);

        return courseResponse;
    }

    public async Task<PrivateFileDataResponse> UpdatePhotoAsync(int userId,
        PrivateFileBlobCreateRequest fileCreateRequest, CancellationToken cancellationToken = default)
    {
        if (fileCreateRequest.CourseId is null)
            throw new ArgumentNullException(nameof(fileCreateRequest.CourseId));

        await _userValidatorManager.ValidateAuthorOfCourseAsync(fileCreateRequest.CourseId.Value, userId,
            cancellationToken);

        PrivateFileDataResponse fileResponse;

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            fileResponse = await _fileService.CreateAsync(fileCreateRequest, true, userId, cancellationToken);

            try
            {
                var successfullyUpdated = await _psqUnitOfWork.CourseRepository.UpdatePhotoAsync(
                    fileCreateRequest.CourseId.Value,
                    fileResponse.Id, cancellationToken: cancellationToken);

                if (!successfullyUpdated)
                {
                    throw new Exception("failed to update photo");
                }
            }
            catch (Exception e)
            {
                await _privateFileService.DeleteAsync(fileResponse.Id);
                throw;
            }

            transaction.Complete();
        }

        return fileResponse;
    }

    public async Task<PrivateFileDataResponse> UpdateVideoAsync(int userId,
        PrivateFileBlobCreateRequest fileCreateRequest, CancellationToken cancellationToken = default)
    {
        if (fileCreateRequest.CourseId is null)
            throw new ArgumentNullException(nameof(fileCreateRequest.CourseId));

        await _userValidatorManager.ValidateAuthorOfCourseAsync(fileCreateRequest.CourseId.Value, userId, cancellationToken);

        PrivateFileDataResponse fileResponse;

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            fileResponse = await _fileService.CreateAsync(fileCreateRequest, true, userId, cancellationToken);

            try
            {
                var successfullyUpdated = await _psqUnitOfWork.CourseRepository.UpdateVideoAsync(fileCreateRequest.CourseId.Value,
                    fileResponse.Id, cancellationToken: cancellationToken);

                if (!successfullyUpdated)
                {
                    throw new Exception("failed to update video");
                }
            }
            catch (Exception e)
            {
                await _privateFileService.DeleteAsync(fileResponse.Id);
                throw;
            }

            transaction.Complete();
        }

        return fileResponse;
    }

    public async Task<CourseUpdateResponse> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var originalCourse = await _psqUnitOfWork.CourseRepository.GetByIdAsync(courseUpdateRequest.Id,
            [nameof(Course.Paragraphs), nameof(Course.Video), nameof(Course.Photo)], cancellationToken: cancellationToken);

        if (originalCourse is null)
            throw new KeyNotFoundException("Cannot find course with such id");

        await _userValidatorManager.ValidateAuthorOfCourseAsync(courseUpdateRequest.Id, userId, cancellationToken);

        var course = _mapper.Map(courseUpdateRequest, originalCourse);

        course = await _psqUnitOfWork.CourseRepository.UpdateAsync(course, cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        var courseResponse = _mapper.Map<CourseUpdateResponse>(course);

        return courseResponse;
    }

    public async Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        await _userValidatorManager.ValidateAuthorOfCourseAsync(id, userId, cancellationToken);

        var result = await _psqUnitOfWork.CourseRepository.DeleteAsync(id, cancellationToken);

        if (result)
            return;

        throw new Exception($"Errors while deleting course with Id: {id}");
    }

    public async Task<bool> DeletePhotoAsync(int courseId, int userId, CancellationToken cancellationToken = default)
    {
        await _userValidatorManager.ValidateAuthorOfCourseAsync(courseId, userId, cancellationToken);

        bool result;

        var photoId =
            await _psqUnitOfWork.CourseRepository.GetPhotoIdAsync(courseId, cancellationToken: cancellationToken);

        if (!photoId.HasValue)
            return true;

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            await _psqUnitOfWork.CourseRepository.UpdatePhotoAsync(courseId, null, cancellationToken);
            result = await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(photoId.Value, cancellationToken);
            transaction.Complete();
        }

        return result;
    }

    public async Task<bool> DeleteVideoAsync(int courseId, int userId, CancellationToken cancellationToken = default)
    {
        await _userValidatorManager.ValidateAuthorOfCourseAsync(courseId, userId, cancellationToken);

        bool result;

        var videoId =
            await _psqUnitOfWork.CourseRepository.GetPhotoIdAsync(courseId, cancellationToken: cancellationToken);

        if (!videoId.HasValue)
            return true;

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            await _psqUnitOfWork.CourseRepository.UpdateVideoAsync(courseId, null, cancellationToken);
            result = await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(videoId.Value, cancellationToken);
            transaction.Complete();
        }

        return result;
    }
}