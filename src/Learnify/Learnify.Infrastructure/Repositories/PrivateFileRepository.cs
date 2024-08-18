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

    public async Task<PrivateFileBlobInfoResponse> GetByIdAsync(int id)
    {
        var fileData = await _context.FileDatas.FindAsync(id);

        if (fileData is null)
            return null;

        return _mapper.Map<PrivateFileBlobInfoResponse>(fileData);
    }

    public async Task<IEnumerable<PrivateFileBlobInfoResponse>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id)).ToArrayAsync();
        
        if(fileDatas.Length != ids.Count())
            return null;
        
        return _mapper.Map<IEnumerable<PrivateFileBlobInfoResponse>>(fileDatas);
    }

    public async Task<PrivateFileDataResponse> CreateFileAsync(PrivateFileDataCreateRequest privateFileDataCreateRequest)
    {
        var fileDataToCreate = _mapper.Map<PrivateFileData>(privateFileDataCreateRequest);

        await _context.FileDatas.AddAsync(fileDataToCreate);

        return _mapper.Map<PrivateFileDataResponse>(fileDataToCreate);
    }

    public async Task<IEnumerable<PrivateFileDataResponse>> CreateFilesAsync(IEnumerable<PrivateFileDataCreateRequest> fileDataCreateRequests)
    {
        var fileDatasToCreate = _mapper.Map<IEnumerable<PrivateFileData>>(fileDataCreateRequests);
        
        await _context.FileDatas.AddRangeAsync(fileDatasToCreate);

        return _mapper.Map<IEnumerable<PrivateFileDataResponse>>(fileDatasToCreate);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var fileData = await _context.FileDatas.FindAsync(id);
        
        if(fileData is null)
            return false;

        _context.FileDatas.Remove(fileData);
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var fileDatas = await _context.FileDatas.Where(f => ids.Contains(f.Id)).ToArrayAsync();
        
        if(fileDatas.Length != ids.Count())
            return false;

        _context.FileDatas.RemoveRange(fileDatas);
        return true;
    }
}