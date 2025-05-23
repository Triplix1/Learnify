﻿using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.Interfaces;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;
using Learnify.Core.Exceptions;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification.Course;
using Learnify.Core.Specification.Filters;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class CourseService : ICourseService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IUserBoughtValidatorManager _userBoughtValidatorManager;
    private readonly IFileService _fileService;
    private readonly IPrivateFileService _privateFileService;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper,
        IPsqUnitOfWork psqUnitOfWork,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IFileService fileService,
        IPrivateFileService privateFileService,
        IUserBoughtValidatorManager userBoughtValidatorManager)
    {
        _mapper = mapper;
        _psqUnitOfWork = psqUnitOfWork;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _fileService = fileService;
        _privateFileService = privateFileService;
        _userBoughtValidatorManager = userBoughtValidatorManager;
    }

    public async Task<PagedList<CourseTitleResponse>> GetAllCourseTitles(CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var filterCourseParams = new FilterCoursesParams()
        {
            CourseParams = courseParams
        };

        return await GetAllFilteredCourseTitles(filterCourseParams, cancellationToken);
    }

    public async Task<PagedList<CourseTitleResponse>> GetMyCourseTitles(int userId, CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var filterParams = new FilterCoursesParams()
        {
            CourseParams = courseParams,
            AuthorId = userId,
            PublishedOnly = false
        };

        return await GetAllFilteredCourseTitles(filterParams, cancellationToken);
    }

    public async Task<PagedList<CourseTitleResponse>> GetMySubscribedCourseTitles(int userId, CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var filteredCourseParams = new FilterCoursesParams()
        {
            CourseParams = courseParams,
            PublishedOnly = false,
            UserId = userId,
        };

        return await GetAllFilteredCourseTitles(filteredCourseParams, cancellationToken);
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

    public async Task<CourseMainInfoResponse> GetMainInfoResponseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        var includes = new[] { nameof(Course.Paragraphs), nameof(Course.Video), nameof(Course.Author) };

        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(courseId, includes, cancellationToken);

        var response = _mapper.Map<CourseMainInfoResponse>(course);

        response.UserHasBoughtThisCourse = await GetUserHasAccessToTheCourseValue(courseId, userId, cancellationToken);

        return response;
    }

    private async Task<bool> GetUserHasAccessToTheCourseValue(int courseId, int userId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _userBoughtValidatorManager.ValidateUserAccessToTheCourseAsync(userId, courseId, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<CourseStudyResponse> GetCourseStudyResponseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userBoughtValidatorManager.ValidateUserAccessToTheCourseAsync(userId, courseId, cancellationToken);

        return await _psqUnitOfWork.CourseRepository.GetCourseStudyResponseAsync(courseId, cancellationToken);
    }

    public async Task<CourseResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var includes = new[] { nameof(Course.Paragraphs), nameof(Course.Video), nameof(Course.Photo) };

        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id, includes, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return courseResponse;
    }

    public async Task<CourseUpdateResponse> GetForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        var includes = new[] { nameof(Course.Paragraphs), nameof(Course.Video), nameof(Course.Photo) };

        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id, includes, cancellationToken);

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

        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(fileCreateRequest.CourseId.Value, userId,
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

        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(fileCreateRequest.CourseId.Value, userId,
            cancellationToken);

        PrivateFileDataResponse fileResponse;

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            fileResponse = await _fileService.CreateAsync(fileCreateRequest, true, userId, cancellationToken);

            try
            {
                var successfullyUpdated = await _psqUnitOfWork.CourseRepository.UpdateVideoAsync(
                    fileCreateRequest.CourseId.Value,
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
            [nameof(Course.Paragraphs), nameof(Course.Video), nameof(Course.Photo)],
            cancellationToken: cancellationToken);

        if (originalCourse is null)
            throw new KeyNotFoundException("Cannot find course with such id");

        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(courseUpdateRequest.Id, userId,
            cancellationToken);

        var course = _mapper.Map(courseUpdateRequest, originalCourse);

        course = await _psqUnitOfWork.CourseRepository.UpdateAsync(course, cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        var courseResponse = _mapper.Map<CourseUpdateResponse>(course);

        return courseResponse;
    }

    public async Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(id, userId, cancellationToken);

        var result = await _psqUnitOfWork.CourseRepository.DeleteAsync(id, cancellationToken);

        if (result)
            return;

        throw new Exception($"Errors while deleting course with Id: {id}");
    }

    public async Task<bool> DeletePhotoAsync(int courseId, int userId, CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(courseId, userId, cancellationToken);

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
        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(courseId, userId, cancellationToken);

        bool result;

        var videoId =
            await _psqUnitOfWork.CourseRepository.GetVideoIdAsync(courseId, cancellationToken: cancellationToken);

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

    private async Task<PagedList<CourseTitleResponse>> GetAllFilteredCourseTitles(
        FilterCoursesParams filterCoursesParams,
        CancellationToken cancellationToken = default)
    {
        var filter = _mapper.Map<EfFilter<Course>>(filterCoursesParams.CourseParams);

        if (filterCoursesParams.CourseParams.Search is not null)
            filter.Specification &= new SearchCourseSpecification(filterCoursesParams.CourseParams.Search);

        if (filterCoursesParams.AuthorId is not null)
            filter.Specification &= new CourseAuthorSpecification(filterCoursesParams.AuthorId.Value);

        if (filterCoursesParams.PublishedOnly)
            filter.Specification &= new CourseOnlyPublishedSpecification();

        if (filterCoursesParams.UserId is not null)
            filter.Specification &= new UserSubscribedToTheCourseSpecification(filterCoursesParams.UserId.Value);

        filter.Includes = [c => c.Photo];

        var courses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter, cancellationToken);

        var courseTitleResponses = _mapper.Map<IEnumerable<CourseTitleResponse>>(courses.Items);

        var courseTitles = new PagedList<CourseTitleResponse>(courseTitleResponses, courses.TotalCount,
            courses.CurrentPage, courses.PageSize);

        return courseTitles;
    }
}