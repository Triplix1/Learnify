using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.File;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class PrivateFileRepository : IPrivateFileRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PrivateFileRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PrivateFileData> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fileData = await _context.FileDatas.FindAsync([id], cancellationToken);

        if (fileData is null)
            return null;

        return fileData;
    }

    public async Task<IEnumerable<PrivateFileData>> GetByIdsAsync(IEnumerable<int> ids,
        CancellationToken cancellationToken = default)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id))
            .ToArrayAsync(cancellationToken: cancellationToken);

        if (fileDatas.Length != ids.Count())
            return null;

        return fileDatas;
    }

    public async Task<PrivateFileData> CreateFileAsync(PrivateFileData privateFileDataCreateRequest,
        CancellationToken cancellationToken = default)
    {
        await _context.FileDatas.AddAsync(privateFileDataCreateRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return privateFileDataCreateRequest;
    }

    public async Task<IEnumerable<PrivateFileData>> CreateFilesAsync(
        IEnumerable<PrivateFileData> fileDataCreateRequests, CancellationToken cancellationToken = default)
    {
        await _context.FileDatas.AddRangeAsync(fileDataCreateRequests, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return fileDataCreateRequests;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fileData = await _context.FileDatas.FindAsync([id], cancellationToken);

        if (fileData is null)
            return false;

        _context.FileDatas.Remove(fileData);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id))
            .ToArrayAsync(cancellationToken: cancellationToken);

        if (fileDatas.Length != ids.Count())
            return false;

        _context.FileDatas.RemoveRange(fileDatas);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}