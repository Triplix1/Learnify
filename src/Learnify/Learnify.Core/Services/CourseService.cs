using AutoMapper;
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
public class CourseService: ICourseService
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
    
    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync(CourseParams courseParams)
    {
        var filter = _mapper.Map<EfFilter<Course>>(courseParams);

        if (courseParams.Search is not null)
            filter.Specification = new SearchCourseSpecification(courseParams.Search);
            
        var courses = await _psqUnitOfWork.CourseRepository.GetFilteredAsync(filter);
        return ApiResponse<IEnumerable<CourseResponse>>.Success(_mapper.Map<IEnumerable<CourseResponse>>(courses));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> GetByIdAsync(int id)
    {
        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id);

        return ApiResponse<CourseResponse>.Success(course);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int userId)
    {
        var course = await _psqUnitOfWork.CourseRepository.CreateAsync(courseCreateRequest, userId);
        
        return ApiResponse<CourseResponse>.Success(course);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId)
    {
        var validationResult = await ValidateAuthorOfCourseAsync(courseUpdateRequest.Id, userId);

        if (validationResult is not null)
            return ApiResponse<CourseResponse>.Failure(validationResult);
        
        var response = await _psqUnitOfWork.CourseRepository.UpdateAsync(courseUpdateRequest);

        if (response is null)
            return ApiResponse<CourseResponse>.Failure(new KeyNotFoundException("Cannot find course with such Id"));

        return ApiResponse<CourseResponse>.Success(response);
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(int id, int userId)
    {
        var validationResult = await ValidateAuthorOfCourseAsync(id, userId);

        if (validationResult is not null)
            return ApiResponse.Failure(validationResult);
        
        var result = await _psqUnitOfWork.CourseRepository.DeleteAsync(id);
        
        if(result) 
            return ApiResponse.Success();

        return ApiResponse.Failure(new Exception($"Errors while deleting course with Id: {id}"));
    }
    
    private async Task<Exception> ValidateAuthorOfCourseAsync(int courseId, int userId)
    {
        var authorId = await _psqUnitOfWork.CourseRepository.GetAuthorId(courseId);

        if (authorId is null)
            return new KeyNotFoundException("Cannot find course with such Id");
            
        if(authorId != userId)
            return new Exception("You have not permissions to update this course");

        return null;
    }
}