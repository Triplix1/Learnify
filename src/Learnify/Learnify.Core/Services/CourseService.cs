using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Custom;

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
    public async Task<ApiResponse<CourseResponse>> GetById(int id)
    {
        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id);

        return ApiResponse<CourseResponse>.Success(course);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int authorId)
    {
        var course = await _psqUnitOfWork.CourseRepository.CreateAsync(courseCreateRequest, authorId);

        await _psqUnitOfWork.SaveChangesAsync();

        return ApiResponse<CourseResponse>.Success(course);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int authorId)
    {
        var response = await _psqUnitOfWork.CourseRepository.UpdateAsync(courseUpdateRequest);

        if (response is null)
            return ApiResponse<CourseResponse>.Failure(new KeyNotFoundException("Cannot find course with such Id"));
        
        if(response.AuthorId != authorId)
            return ApiResponse<CourseResponse>.Failure(new Exception("You have not permissions to update this course"));

        await _psqUnitOfWork.SaveChangesAsync();
        return ApiResponse<CourseResponse>.Success(response);
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var result = await _psqUnitOfWork.CourseRepository.DeleteAsync(id);
        
        if(result) 
            return ApiResponse.Success();

        return ApiResponse.Failure(new Exception($"Errors while deleting course with Id: {id}"));
    }
}