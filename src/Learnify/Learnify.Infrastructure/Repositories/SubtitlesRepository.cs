using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Subtitles;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class SubtitlesRepository: ISubtitlesRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SubtitlesRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Subtitle> GetByIdAsync(int id)
    {
        var subtitle = await _context.Subtitles.FindAsync(id);

        return subtitle;
    }

    public async Task<IEnumerable<Subtitle>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var subtitles = await _context.Subtitles.Where(s => ids.Contains(s.Id)).ToArrayAsync();
        
        return subtitles;
    }

    public async Task<Subtitle> CreateAsync(Subtitle subtitlesCreateRequest)
    {
        await _context.Subtitles.AddAsync(subtitlesCreateRequest);
        await _context.SaveChangesAsync();
        
        return subtitlesCreateRequest;
    }

    public async Task<IEnumerable<Subtitle>> CreateRangeAsync(IEnumerable<Subtitle> subtitlesCreateRequest)
    {
        await _context.Subtitles.AddRangeAsync(subtitlesCreateRequest);
        await _context.SaveChangesAsync();

        return subtitlesCreateRequest;
    }

    public async Task<Subtitle> UpdateAsync(Subtitle subtitlesUpdateRequest)
    {
        var subtitle = await _context.Subtitles.FindAsync(subtitlesUpdateRequest.Id);

        if (subtitle is null)
            return null;

        _mapper.Map(subtitlesUpdateRequest, subtitle);
        
        _context.Subtitles.Update(subtitle);
        await _context.SaveChangesAsync();
        
        return subtitle;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subtitle = await _context.Subtitles.FindAsync(id);

        if (subtitle is null)
            return false;

        _context.Subtitles.Remove(subtitle);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var subtitles = await _context.Subtitles.Where(s => ids.Contains(s.Id)).ToArrayAsync();

        if (subtitles.Length != ids.Count())
            return false;

        _context.Subtitles.RemoveRange(subtitles);
        await _context.SaveChangesAsync();
        
        return true;
    }
}