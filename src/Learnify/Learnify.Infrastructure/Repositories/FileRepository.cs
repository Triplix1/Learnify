using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.File;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class FileRepository: IFileRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FileRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FileDataResponse> GetByIdAsync(int id)
    {
        var fileData = await _context.FileDatas.FindAsync(id);

        if (fileData is null)
            return null;

        return _mapper.Map<FileDataResponse>(fileData);
    }

    public async Task<IEnumerable<FileDataResponse>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id)).ToArrayAsync();
        
        if(fileDatas.Length != ids.Count())
            return null;
        
        return _mapper.Map<IEnumerable<FileDataResponse>>(fileDatas);
    }

    public async Task<FileDataResponse> CreateFileAsync(FileDataCreateRequest fileDataCreateRequest)
    {
        var fileDataToCreate = _mapper.Map<FileData>(fileDataCreateRequest);

        await _context.FileDatas.AddAsync(fileDataToCreate);

        return _mapper.Map<FileDataResponse>(fileDataToCreate);
    }

    public async Task<IEnumerable<FileDataResponse>> CreateFilesAsync(IEnumerable<FileDataCreateRequest> fileDataCreateRequests)
    {
        var fileDatasToCreate = _mapper.Map<IEnumerable<FileData>>(fileDataCreateRequests);
        
        await _context.FileDatas.AddRangeAsync(fileDatasToCreate);

        return _mapper.Map<IEnumerable<FileDataResponse>>(fileDatasToCreate);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var fileData = await _context.FileDatas.FindAsync(id);
        
        if(fileData is null)
            return false;

        _context.FileDatas.Remove(fileData);
        return true;
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id)).ToArrayAsync();
        
        if(fileDatas.Length == 0)
            return 0;

        _context.FileDatas.RemoveRange(fileDatas);
        return fileDatas.Length;
    }
}