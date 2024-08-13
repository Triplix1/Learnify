using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class ParagraphRepository: IParagraphRepository
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
    public async Task<PagedList<ParagraphResponse>> GetFilteredAsync(EfFilter<Paragraph> filter)
    {
        var query = _context.Paragraphs.AsQueryable();

        query = filter.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter.Specification is not null)
            query = query.Where(filter.Specification.GetExpression());

        var pagedList = await PagedList<Paragraph>.CreateAsync(query, filter.PageNumber, filter.PageSize);

        return _mapper.Map<PagedList<ParagraphResponse>>(pagedList);
    }

    /// <inheritdoc />
    public async Task<ParagraphResponse> GetByIdAsync(int key)
    {
        var paragraph = await _context.Paragraphs.FindAsync(key);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find paragraph with such Id");

        return _mapper.Map<ParagraphResponse>(paragraph);
    }

    /// <inheritdoc />
    public async Task<ParagraphResponse> CreateAsync(ParagraphCreateRequest entity)
    {
        var paragraph = _mapper.Map<Paragraph>(entity);

        await _context.Paragraphs.AddAsync(paragraph);

        return _mapper.Map<ParagraphResponse>(paragraph);
    }

    /// <inheritdoc />
    public async Task<ParagraphResponse> UpdateAsync(ParagraphUpdateRequest entity)
    {
        var paragraph = await _context.Paragraphs.FindAsync(entity.Id);

        if (paragraph is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        _mapper.Map(entity, paragraph);

        _context.Paragraphs.Update(paragraph);
        return _mapper.Map<ParagraphResponse>(paragraph);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var paragraph = await _context.Paragraphs.FindAsync(id);

        if (paragraph is null)
            return false;

        _context.Paragraphs.Remove(paragraph);
        return true;
    }

    /// <inheritdoc />
    public async Task<int> GetAuthorId(int id)
    {
        var course = await _context.Paragraphs.Include(p => p.Course).FirstOrDefaultAsync(p => p.Id == id);

        return course.Course.AuthorId;
    }
}