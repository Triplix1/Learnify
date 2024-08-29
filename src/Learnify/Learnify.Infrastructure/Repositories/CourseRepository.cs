using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
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
    public async Task<Course> GetByIdAsync(int key, IEnumerable<string> includes)
    {
        var courseQuery = includes.Aggregate(_context.Courses.AsQueryable(), (c, include) => c.Include(include));

        var course = await courseQuery.FirstOrDefaultAsync(c => c.Id == key);
        
        return course;
    }

    public async Task<Course> PublishAsync(int key, bool publish)
    {
        var course = await _context.Courses.FindAsync(key);

        if (course is null)
            return null;

        course.IsPublished = publish;

        return course;
    }

    /// <inheritdoc />
    public async Task<Course> CreateAsync(Course courseCreateRequest)
    {
        await _context.Courses.AddAsync(courseCreateRequest);
        await _context.SaveChangesAsync();

        return courseCreateRequest;
    }

    /// <inheritdoc />
    public async Task<Course> UpdateAsync(Course entity)
    {
        var course = await _context.Courses.FindAsync(entity.Id);

        if (course is null)
            return null;

        _mapper.Map(entity, course);
        
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        
        return course;
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
    public async Task<int?> GetAuthorId(int courseId)
    {
        var course = await _context.Courses.FindAsync(courseId);

        if (course is null)
            return null;
        
        return course.AuthorId;

    }

    /// <inheritdoc />
    public async Task<PagedList<Course>> GetFilteredAsync(EfFilter<Course> filter)
    {
        var query = _context.Courses.AsQueryable();

        query = filter.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter.Specification is not null)
            query = query.Where(filter.Specification.GetExpression());

        var pagedList = await PagedList<Course>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        
        return pagedList;
    }
    
    
}