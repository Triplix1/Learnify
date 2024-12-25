using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Subtitles;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class SubtitlesRepository : ISubtitlesRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SubtitlesRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Subtitle> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var subtitle = await _context.Subtitles.FindAsync([id], cancellationToken);

        return subtitle;
    }

    public async Task<IEnumerable<Subtitle>> GetByIdsAsync(IEnumerable<int> ids,
        CancellationToken cancellationToken = default)
    {
        var subtitles = await _context.Subtitles.Where(s => ids.Contains(s.Id))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return subtitles;
    }

    public async Task<Subtitle> CreateAsync(Subtitle subtitlesCreateRequest,
        CancellationToken cancellationToken = default)
    {
        await _context.Subtitles.AddAsync(subtitlesCreateRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return subtitlesCreateRequest;
    }

    public async Task<IEnumerable<Subtitle>> CreateRangeAsync(IEnumerable<Subtitle> subtitlesCreateRequest,
        CancellationToken cancellationToken = default)
    {
        await _context.Subtitles.AddRangeAsync(subtitlesCreateRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return subtitlesCreateRequest;
    }

    public async Task<Subtitle> UpdateAsync(Subtitle subtitlesUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var subtitle = await _context.Subtitles.FindAsync([subtitlesUpdateRequest.Id], cancellationToken);

        if (subtitle is null)
            return null;

        _mapper.Map(subtitlesUpdateRequest, subtitle);

        _context.Subtitles.Update(subtitle);
        await _context.SaveChangesAsync(cancellationToken);

        return subtitle;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var subtitle = await _context.Subtitles.FindAsync([id], cancellationToken);

        if (subtitle is null)
            return false;

        _context.Subtitles.Remove(subtitle);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var subtitles = await _context.Subtitles.Where(s => ids.Contains(s.Id))
            .ToArrayAsync(cancellationToken: cancellationToken);

        if (subtitles.Length != ids.Count())
            return false;

        _context.Subtitles.RemoveRange(subtitles);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}