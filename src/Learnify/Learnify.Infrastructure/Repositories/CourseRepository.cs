using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Courses repo
/// </summary>
public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILessonRepository _lessonRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Courses constructor
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="lessonRepository"><see cref="ILessonRepository"/></param>
    public CourseRepository(ApplicationDbContext context, IMapper mapper, ILessonRepository lessonRepository)
    {
        _context = context;
        _mapper = mapper;
        _lessonRepository = lessonRepository;
    }

    /// <inheritdoc />
    public async Task<CourseResponse> GetByIdAsync(int key)
    {
        var course = await _context.Courses.Include(c => c.Paragraphs).FirstOrDefaultAsync(c => c.Id == key);

        if (course is null)
            return null;

        var courseResponse = _mapper.Map<CourseResponse>(course);
        
        return courseResponse;
    }

    /// <inheritdoc />
    public async Task<CourseResponse> CreateAsync(CourseCreateRequest courseCreateRequest, int authorId)
    {
        var course = _mapper.Map<Course>(courseCreateRequest);

        course.AuthorId = authorId;

        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

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
        await _context.SaveChangesAsync();
        
        return _mapper.Map<CourseResponse>(course);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
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
    
    
}