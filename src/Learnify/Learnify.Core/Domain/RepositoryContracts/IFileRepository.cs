using Learnify.Core.Dto.File;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IFileRepository
{
    Task<FileDataResponse> GetByIdAsync(int id);
    Task<IEnumerable<FileDataResponse>> GetByIdsAsync(IEnumerable<int> ids);
    Task<FileDataResponse> CreateFileAsync(FileDataCreateRequest fileDataCreateRequest);
    Task<IEnumerable<FileDataResponse>> CreateFilesAsync(IEnumerable<FileDataCreateRequest> fileDataCreateRequests);
    Task<bool> DeleteAsync(int id);
    Task<int> DeleteRangeAsync(IEnumerable<int> ids);
}