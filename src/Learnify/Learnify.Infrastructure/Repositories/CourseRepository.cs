using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Learnify.Core.Extensions;

namespace Learnify.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CourseRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Course> GetByIdAsync(int key, IEnumerable<string> includes = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Courses.AsQueryable();
        
        query = query.IncludeFields(includes);

        var course = await query.FirstOrDefaultAsync(c => c.Id == key, cancellationToken: cancellationToken);

        return course;
    }

    public async Task<int> GetCourseAuthorIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.Where(s => s.Id == courseId).Select(c => c.AuthorId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<CoursePaymentResponse> GetCoursePaymentDataAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.Select(c => new CoursePaymentResponse()
        {
            Id = c.Id,
            Name = c.Name,
            Price = c.Price
        }).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return course;
    }

    public async Task<CourseStudyResponse> GetCourseStudyResponseAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var query = _mapper.ProjectTo<CourseStudyResponse>(_context.Courses.Include(c => c.Paragraphs));

        var response = await
            query.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);

        return response;
    }

    public async Task<int?> GetVideoIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.Where(c => c.Id == courseId).Select(s => s.VideoId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<int?> GetPhotoIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.Where(c => c.Id == courseId).Select(s => s.PhotoId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<int?> GetAuthorIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([courseId], cancellationToken);

        if (course is null)
            return null;

        return course.AuthorId;
    }

    public async Task<PagedList<Course>> GetFilteredAsync(EfFilter<Course> filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Courses.AsQueryable();

        query = query.ApplyFilters(filter);

        return await PagedList<Course>.CreateAsync(query, filter.PagedListParams.PageNumber,
            filter.PagedListParams.PageSize, cancellationToken);
    }

    public async Task<Course> CreateAsync(Course courseCreateRequest, CancellationToken cancellationToken = default)
    {
        await _context.Courses.AddAsync(courseCreateRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return courseCreateRequest;
    }

    public async Task<Course> UpdateAsync(Course entity, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([entity.Id], cancellationToken);

        if (course is null)
            return null;

        _mapper.Map(entity, course);

        _context.Courses.Update(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course;
    }

    public async Task<bool> UpdatePhotoAsync(int courseId, int? photoId, CancellationToken cancellationToken = default)
    {
        var updatedCount = await _context.Courses.Where(c => c.Id == courseId)
            .ExecuteUpdateAsync(c => c.SetProperty(cp => cp.PhotoId, photoId), cancellationToken: cancellationToken);

        return updatedCount > 0;
    }

    public async Task<bool> UpdateVideoAsync(int courseId, int? photoId, CancellationToken cancellationToken = default)
    {
        var updatedCount = await _context.Courses.Where(c => c.Id == courseId)
            .ExecuteUpdateAsync(c => c.SetProperty(cp => cp.VideoId, photoId), cancellationToken: cancellationToken);

        return updatedCount > 0;
    }

    public async Task<Course> PublishAsync(int key, bool publish, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([key], cancellationToken);

        if (course is null)
            return null;

        course.IsPublished = publish;

        await _context.SaveChangesAsync(cancellationToken);

        return course;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([id], cancellationToken);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}