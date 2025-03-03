﻿using AutoMapper;
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
        var fileData = await _context.FileDatas.FindAsync([id], cancellationToken);

        return fileData;
    }

    public async Task<PrivateFileData> GetBySubtitleIdAsync(int subtitleId,
        CancellationToken cancellationToken = default)
    {
        var fileData = await _context.Subtitles.Include(s => s.SubtitleFile).Where(s => s.Id == subtitleId)
            .Select(s => s.SubtitleFile).FirstOrDefaultAsync(cancellationToken: cancellationToken);

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

    public async Task<PrivateFileData> CreateFileAsync(PrivateFileCreateRequest privateFileDataCreateRequest,
        CancellationToken cancellationToken = default)
    {
        var privateFileData = _mapper.Map<PrivateFileData>(privateFileDataCreateRequest);

        await _context.FileDatas.AddAsync(privateFileData, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return privateFileData;
    }

    public async Task<PrivateFileData> UpdateFileAsync(PrivateFileData file,
        CancellationToken cancellationToken = default)
    {
        var privateFile = await _context.FileDatas.FindAsync([file.Id], cancellationToken);

        if (privateFile == null)
            throw new KeyNotFoundException();

        _mapper.Map(file, privateFile);

        _context.Update(file);
        await _context.SaveChangesAsync(cancellationToken);

        return file;
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