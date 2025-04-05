using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Extensions;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class ParagraphRepository : IParagraphRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ParagraphRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedList<Paragraph>> GetFilteredAsync(EfFilter<Paragraph> filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Paragraphs.AsQueryable();

        query = filter.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter.Specification is not null)
            query = query.Where(filter.Specification.GetExpression());

        var pagedList =
            await PagedList<Paragraph>.CreateAsync(query, filter.PagedListParams.PageNumber, filter.PagedListParams.PageSize, cancellationToken);

        return pagedList;
    }

    public async Task<Paragraph> GetByIdAsync(int key, IEnumerable<string> stringToInclude = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Paragraphs.AsQueryable();
        query = query.IncludeFields(stringToInclude);

        var paragraph = await query.FirstOrDefaultAsync(x => x.Id == key, cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with such Id");

        return paragraph;
    }

    public async Task<int?> GetCourseIdAsync(int paragraphId, CancellationToken cancellationToken = default)
    {
        return await _context.Paragraphs.Where(x => x.Id == paragraphId).Select(x => x.CourseId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Paragraph> CreateAsync(Paragraph entity, CancellationToken cancellationToken = default)
    {
        await _context.Paragraphs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Paragraph> UpdateAsync(Paragraph entity, CancellationToken cancellationToken = default)
    {
        var paragraph = await _context.Paragraphs.FindAsync([entity.Id], cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        _mapper.Map(entity, paragraph);

        _context.Paragraphs.Update(paragraph);
        await _context.SaveChangesAsync(cancellationToken);

        return paragraph;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var paragraph = await _context.Paragraphs.FindAsync([id], cancellationToken);

        if (paragraph is null)
            return false;

        _context.Paragraphs.Remove(paragraph);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int?> GetAuthorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var authorId = await _context.Paragraphs.Include(p => p.Course).Where(p => p.Id == id)
            .Select(p => p.Course.AuthorId)
            .SingleOrDefaultAsync(cancellationToken);

        return authorId;
    }
}