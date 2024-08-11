using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Specification;
using Learnify.Infrastructure.Data;
using Learnify.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Courses repo
/// </summary>
public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMongoAppDbContext _mongoContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Courses constructor
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="mongoContext"><see cref="IMongoAppDbContext"/></param>
    public CourseRepository(ApplicationDbContext context, IMapper mapper, IMongoAppDbContext mongoContext)
    {
        _context = context;
        _mapper = mapper;
        _mongoContext = mongoContext;
    }

    /// <inheritdoc />
    public async Task<CourseResponse> GetByIdAsync(int key)
    {
        var course = await _context.Courses.FindAsync(key);

        if (course is null)
            return null;

        var courseResponse = _mapper.Map<CourseResponse>(course);

        foreach (var paragraph in courseResponse.Paragraphs)
        {
            
        }
        
        return ;
    }

    /// <inheritdoc />
    public async Task<CourseResponse> CreateAsync(CourseCreateRequest courseCreateRequest, int authorId)
    {
        var course = _mapper.Map<Course>(courseCreateRequest);

        course.AuthorId = authorId;

        await _context.Courses.AddAsync(course);

        return _mapper.Map<CourseResponse>(course);
    }

    /// <inheritdoc />
    public async Task<CourseResponse> UpdateAsync(CourseUpdateRequest entity)
    {
        var course = await _context.Courses.FindAsync(entity.Id);

        if (course is null)
            return null;

        _mapper.Map(entity, course);
        
        _context.Courses.Update(course);
        return _mapper.Map<CourseResponse>(course);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        return true;
    }

    /// <inheritdoc />
    public async Task<PagedList<CourseResponse>> GetFilteredAsync(EfFilter<Course> filter)
    {
        var query = _context.Courses.AsQueryable();

        query = filter.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter.Specification is not null)
            query = query.Where(filter.Specification.GetExpression());

        var pagedList = await PagedList<Course>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        
        return _mapper.Map<PagedList<CourseResponse>>(pagedList);
    }

    public Task<IEnumerable<string>> GetLessonIds(int courseId)
    {
        _context.Courses.SelectMany(c => c.Paragraphs).SelectMany(p => p.)
    }
}