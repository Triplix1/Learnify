using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IPrivateFileRepository
{
    Task<PrivateFileData> GetByIdAsync(int id);
    Task<IEnumerable<PrivateFileData>> GetByIdsAsync(IEnumerable<int> ids);
    Task<PrivateFileData> CreateFileAsync(PrivateFileData privateFileDataCreateRequest);
    Task<IEnumerable<PrivateFileData>> CreateFilesAsync(IEnumerable<PrivateFileData> fileDataCreateRequests);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
}