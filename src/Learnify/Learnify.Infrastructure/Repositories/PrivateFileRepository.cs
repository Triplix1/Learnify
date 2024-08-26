using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.File;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class PrivateFileRepository: IPrivateFileRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PrivateFileRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PrivateFileData> GetByIdAsync(int id)
    {
        var fileData = await _context.FileDatas.FindAsync(id);

        if (fileData is null)
            return null;

        return fileData;
    }

    public async Task<IEnumerable<PrivateFileData>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id)).ToArrayAsync();
        
        if(fileDatas.Length != ids.Count())
            return null;
        
        return fileDatas;
    }

    public async Task<PrivateFileData> CreateFileAsync(PrivateFileData privateFileDataCreateRequest)
    { 
        await _context.FileDatas.AddAsync(privateFileDataCreateRequest);
        await _context.SaveChangesAsync();

        return privateFileDataCreateRequest;
    }

    public async Task<IEnumerable<PrivateFileData>> CreateFilesAsync(IEnumerable<PrivateFileData> fileDataCreateRequests)
    {
        await _context.FileDatas.AddRangeAsync(fileDataCreateRequests);
        await _context.SaveChangesAsync();

        return fileDataCreateRequests;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var fileData = await _context.FileDatas.FindAsync(id);
        
        if(fileData is null)
            return false;

        _context.FileDatas.Remove(fileData);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id)).ToArrayAsync();
        
        if(fileDatas.Length != ids.Count())
            return false;

        _context.FileDatas.RemoveRange(fileDatas);
        await _context.SaveChangesAsync();
        
        return true;
    }
}