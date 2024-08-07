using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
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
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a
    /// </summary>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="mongoUnitOfWork"><see cref="IMongoUnitOfWork"/></param>
    public CourseService(IMapper mapper, IMongoUnitOfWork mongoUnitOfWork)
    {
        _mapper = mapper;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync(CourseParams courseParams)
    {
        var filter = _mapper.Map<MongoFilter<Course>>(courseParams);

        if (courseParams.Search is not null)
            filter.Specification = new SearchCourseSpecification(courseParams.Search);
            
        var courses = await _mongoUnitOfWork.CourseRepository.GetFilteredAsync(filter);
        return ApiResponse<IEnumerable<CourseResponse>>.Success(_mapper.Map<IEnumerable<CourseResponse>>(courses));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> GetById(string id)
    {
        var course = await _mongoUnitOfWork.CourseRepository.GetByIdAsync(id);

        return ApiResponse<CourseResponse>.Success(_mapper.Map<CourseResponse>(course));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int authorId)
    {
        var course = _mapper.Map<Course>(courseCreateRequest);

        course.AuthorId = authorId;
        
        course = await _mongoUnitOfWork.CourseRepository.CreateAsync(course);

        return ApiResponse<CourseResponse>.Success(_mapper.Map<CourseResponse>(course));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int authorId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}