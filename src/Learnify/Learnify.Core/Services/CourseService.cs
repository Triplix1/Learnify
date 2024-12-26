using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Specification.Custom;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Services;

public class CourseService : ICourseService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IUserValidatorManager _userValidatorManager;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IPsqUnitOfWork psqUnitOfWork, IUserValidatorManager userValidatorManager)
    {
        _mapper = mapper;
        _psqUnitOfWork = psqUnitOfWork;
        _userValidatorManager = userValidatorManager;
    }

    public async Task<PagedList<CourseTitleResponse>> GetAllCourseTitles(
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

        return courseTitles;
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
        var includes = new[] { nameof(Course.Paragraphs) };

        var course = await _psqUnitOfWork.CourseRepository.GetByIdAsync(id, includes, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return courseResponse;
    }

    public async Task<CourseResponse> PublishAsync(int id, bool publish, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userValidatorManager.ValidateAuthorOfCourseAsync(id, userId, cancellationToken);

        var course = await _psqUnitOfWork.CourseRepository.PublishAsync(id, publish, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return courseResponse;
    }

    public async Task<CourseResponse> CreateAsync(CourseCreateRequest courseCreateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var course = _mapper.Map<Course>(courseCreateRequest);
        course.AuthorId = userId;
        course.IsPublished = false;

        course = await _psqUnitOfWork.CourseRepository.CreateAsync(course, cancellationToken);

        var courseResponse = _mapper.Map<CourseResponse>(course);

        return courseResponse;
    }

    public async Task<CourseResponse> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var originalCourse = await _psqUnitOfWork.CourseRepository.GetByIdAsync(courseUpdateRequest.Id, [nameof(Course.Paragraphs)], cancellationToken:cancellationToken);

        if (originalCourse is null)
            throw new KeyNotFoundException("Cannot find course with such id");
        
        await _userValidatorManager.ValidateAuthorOfCourseAsync(courseUpdateRequest.Id, userId, cancellationToken);

        var course = _mapper.Map(courseUpdateRequest, originalCourse);

        course = await _psqUnitOfWork.CourseRepository.UpdateAsync(course, cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        var courseResponse = _mapper.Map<CourseResponse>(course);

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
}