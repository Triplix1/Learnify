using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Learnify.Core.Extensions;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Courses repo
/// </summary>
public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Courses constructor
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="lessonRepository"><see cref="ILessonRepository"/></param>
    public CourseRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Course> GetByIdAsync(int key, IEnumerable<string> includes,
        CancellationToken cancellationToken = default)
    {
        var courseQuery = includes.Aggregate(_context.Courses.AsQueryable(), (c, include) => c.Include(include));

        var course = await courseQuery.FirstOrDefaultAsync(c => c.Id == key, cancellationToken: cancellationToken);

        return course;
    }

    public async Task<Course> PublishAsync(int key, bool publish, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var course = await _context.Courses.FindAsync([key], cancellationToken);

        if (course is null)
            return null;

        course.IsPublished = publish;

        await _context.SaveChangesAsync(cancellationToken);

        return course;
    }

    /// <inheritdoc />
    public async Task<Course> CreateAsync(Course courseCreateRequest, CancellationToken cancellationToken = default)
    {
        await _context.Courses.AddAsync(courseCreateRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return courseCreateRequest;
    }

    /// <inheritdoc />
    public async Task<Course> UpdateAsync(Course entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var course = await _context.Courses.FindAsync([entity.Id], cancellationToken);

        if (course is null)
            return null;

        _mapper.Map(entity, course);

        _context.Courses.Update(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var course = await _context.Courses.FindAsync([id], cancellationToken);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<int?> GetAuthorIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var course = await _context.Courses.FindAsync([courseId], cancellationToken);

        if (course is null)
            return null;

        return course.AuthorId;
    }

    /// <inheritdoc />
    public async Task<PagedList<Course>> GetFilteredAsync(EfFilter<Course> filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Courses.AsQueryable();

        if (filter.Includes is not null)
            query = filter.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter.Specification is not null)
            query = query.Where(filter.Specification.GetExpression());

        if (filter.OrderByParams is not null)
        {
            if (!filter.OrderByParams.Asc.HasValue || filter.OrderByParams.Asc.Value)
                query = query.OrderBy(filter.OrderByParams.OrderBy);
            else
                query = query.OrderByDescending(filter.OrderByParams.OrderBy);
        }

        var pagedList =
            await PagedList<Course>.CreateAsync(query, filter.PageNumber, filter.PageSize, cancellationToken);

        return pagedList;
    }
}