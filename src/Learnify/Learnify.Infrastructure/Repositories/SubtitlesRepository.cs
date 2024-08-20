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

    public async Task<SubtitlesResponse> GetByIdAsync(int id)
    {
        var subtitle = await _context.Subtitles.FindAsync(id);

        if (subtitle is null)
            return null;

        return _mapper.Map<SubtitlesResponse>(subtitle);
    }

    public async Task<IEnumerable<SubtitlesResponse>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var subtitles = _context.Subtitles.Where(s => ids.Contains(s.Id));

        var result = _mapper.ProjectTo<SubtitlesResponse>(subtitles);

        return await result.ToArrayAsync();
    }

    public async Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest)
    {
        var subtitle = _mapper.Map<Subtitle>(subtitlesCreateRequest);
        
        await _context.Subtitles.AddAsync(subtitle);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<SubtitlesResponse>(subtitle);

        return response;
    }

    public async Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest)
    {
        var subtitle = await _context.Subtitles.FindAsync(subtitlesUpdateRequest.Id);

        if (subtitle is null)
            return null;

        _mapper.Map(subtitlesUpdateRequest, subtitle);
        
        _context.Subtitles.Update(subtitle);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<SubtitlesResponse>(subtitle);
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