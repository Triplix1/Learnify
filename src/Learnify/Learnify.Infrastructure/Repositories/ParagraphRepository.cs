using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Learnify.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class ParagraphRepository : IParagraphRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public ParagraphRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<Paragraph> GetByIdAsync(int key, IEnumerable<string> stringToInclude = null,
        CancellationToken cancellationToken = default)
    {
        var query = IncludeParamsHelper.IncludeStrings(stringToInclude, _context.Paragraphs);

        var paragraph = await query.FirstOrDefaultAsync(x => x.Id == key, cancellationToken);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with such Id");

        return paragraph;
    }

    /// <inheritdoc />
    public async Task<Paragraph> CreateAsync(Paragraph entity, CancellationToken cancellationToken = default)
    {
        await _context.Paragraphs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var paragraph = await _context.Paragraphs.FindAsync([id], cancellationToken);

        if (paragraph is null)
            return false;

        _context.Paragraphs.Remove(paragraph);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <inheritdoc />
    public async Task<int?> GetAuthorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var authorId = await _context.Paragraphs.Include(p => p.Course).Where(p => p.Id == id)
            .Select(p => p.Course.AuthorId)
            .SingleOrDefaultAsync(cancellationToken);

        return authorId;
    }
}