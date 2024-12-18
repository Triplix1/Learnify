﻿using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification.Custom;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Services;

/// <inheritdoc />
public class CourseService : ICourseService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a
    /// </summary>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="psqUnitOfWork"><see cref="IPsqUnitOfWork"/></param>
    public CourseService(IMapper mapper, IPsqUnitOfWork psqUnitOfWork)
    {
        _mapper = mapper;
        _psqUnitOfWork = psqUnitOfWork;
    }

    public async Task<ApiResponse<PagedList<CourseTitleResponse>>> GetAllCourseTitles(
        CancellationToken cancellationToken = default)
    {
        var filter = new EfFilter<Course>
        {
            PageNumber = 1,
            PageSize = 5,
            OrderByParams = new OrderByParams()
            {
                Asc = false,
                OrderBy = nameof(Course.CreatedAt)
            }
        };

        var courses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter, cancellationToken);

        var courseTitleResponses = _mapper.Map<IEnumerable<CourseTitleResponse>>(courses.Items);

        var courseTitles = new PagedList<CourseTitleResponse>(courseTitleResponses, courses.TotalCount,
            courses.CurrentPage, courses.PageSize);

        return ApiResponse<PagedList<CourseTitleResponse>>.Success(courseTitles);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync(CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var filter = _mapper.Map<EfFilter<Course>>(courseParams);

        if (courseParams.Search is not null)
            filter.Specification = new SearchCourseSpecification(courseParams.Search);

        var courses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter, cancellationToken);
        return ApiResponse<IEnumerable<CourseResponse>>.Success(_mapper.Map<IEnumerable<CourseResponse>>(courses));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var includes = new[] { nameof(Course.Paragraphs) };

        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id, includes, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return ApiResponse<CourseResponse>.Success(courseResponse);
    }

    public async Task<ApiResponse<CourseResponse>> PublishAsync(int id, bool publish, int userId,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateAuthorOfCourseAsync(id, userId, cancellationToken);

        if (validationResult is not null)
            return ApiResponse<CourseResponse>.Failure(validationResult);

        var course = await _psqUnitOfWork.CourseRepository.PublishAsync(id, publish, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return ApiResponse<CourseResponse>.Success(courseResponse);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var course = _mapper.Map<Course>(courseCreateRequest);
        course.AuthorId = userId;
        course.IsPublished = false;

        course = await _psqUnitOfWork.CourseRepository.CreateAsync(course, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return ApiResponse<CourseResponse>.Success(courseResponse);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateAuthorOfCourseAsync(courseUpdateRequest.Id, userId, cancellationToken);

        if (validationResult is not null)
            return ApiResponse<CourseResponse>.Failure(validationResult);

        var course = _mapper.Map<Course>(courseUpdateRequest);

        course = await _psqUnitOfWork.CourseRepository.UpdateAsync(course, cancellationToken);

        if (course is null)
            return ApiResponse<CourseResponse>.Failure(new KeyNotFoundException("Cannot find course with such Id"));

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return ApiResponse<CourseResponse>.Success(courseResponse);
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateAuthorOfCourseAsync(id, userId, cancellationToken);

        if (validationResult is not null)
            return ApiResponse.Failure(validationResult);

        var result = await _psqUnitOfWork.CourseRepository.DeleteAsync(id, cancellationToken);

        if (result)
            return ApiResponse.Success();

        return ApiResponse.Failure(new Exception($"Errors while deleting course with Id: {id}"));
    }

    private async Task<Exception> ValidateAuthorOfCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        var authorId = await _psqUnitOfWork.CourseRepository.GetAuthorIdAsync(courseId, cancellationToken);

        if (authorId is null)
            return new KeyNotFoundException("Cannot find course with such Id");

        if (authorId != userId)
            return new Exception("You have not permissions to update this course");

        return null;
    }
}