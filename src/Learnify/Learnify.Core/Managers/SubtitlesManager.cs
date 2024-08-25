using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Managers;

public class SubtitlesManager: ISubtitlesManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public SubtitlesManager(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mapper = mapper;
    }

    public async Task<SubtitlesResponse> GetSubtitleByIdAsync(int id)
    {
        var subtitle = await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(id);

        var response = _mapper.Map<SubtitlesResponse>(subtitle);
        
        return response;
    }

    public async Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest)
    {
        var subtitle = _mapper.Map<Subtitle>(subtitlesCreateRequest);

        subtitle = await _psqUnitOfWork.SubtitlesRepository.CreateAsync(subtitle);

        var response = _mapper.Map<SubtitlesResponse>(subtitle);

        return response;
    }

    public async Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest)
    {
         var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(subtitlesUpdateRequest.Id);

         if (subtitles is null)
             throw new KeyNotFoundException("Cannot find subtitles with such id");

         var subtitle = _mapper.Map<Subtitle>(subtitlesUpdateRequest);

         if (subtitles.FileId.HasValue && subtitles.FileId != subtitlesUpdateRequest.FileId)
         {
             var oldFile = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(subtitles.FileId.Value);
             
             using var ts = TransactionScopeBuilder.CreateRepeatableReadAsync();

             await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(oldFile.Id);
             
             subtitle = await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle);
             
             await _blobStorage.DeleteAsync(oldFile.ContainerName, oldFile.BlobName);
             
             ts.Complete();
         }
         else
         {
             subtitle = await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle);
         }

         var response = _mapper.Map<SubtitlesResponse>(subtitle);

         return response;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(id);

        if (subtitles is null)
            return false;

        if (subtitles.FileId.HasValue)
        {
            var oldFile = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(subtitles.FileId.Value);
            
            using var ts = TransactionScopeBuilder.CreateRepeatableReadAsync();

            await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(oldFile.Id);
            
            var result = await _psqUnitOfWork.SubtitlesRepository.DeleteAsync(subtitles.Id);

            await _blobStorage.DeleteAsync(oldFile.ContainerName, oldFile.BlobName);
             
            ts.Complete();

            return result;
        }
        
        return await _psqUnitOfWork.SubtitlesRepository.DeleteAsync(subtitles.Id);
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdsAsync(ids);

        if (subtitles is null)
            return false;

        var fileIds = subtitles.Where(s => s.FileId.HasValue).Select(s => s.FileId.Value);
        
        if (fileIds.Any())
        {
            var files = await _psqUnitOfWork.PrivateFileRepository.GetByIdsAsync(fileIds);
            
            using var ts = TransactionScopeBuilder.CreateRepeatableReadAsync();

            await _psqUnitOfWork.PrivateFileRepository.DeleteRangeAsync(fileIds);
            
            var result = await _psqUnitOfWork.SubtitlesRepository.DeleteRangeAsync(ids);

            foreach (var file in files)
            {
                await _blobStorage.DeleteAsync(file.ContainerName, file.BlobName);
            }
             
            ts.Complete();

            return result;
        }
        
        return await _psqUnitOfWork.SubtitlesRepository.DeleteRangeAsync(ids);
    }
}