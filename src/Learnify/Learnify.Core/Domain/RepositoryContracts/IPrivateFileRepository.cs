using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IPrivateFileRepository
{
    Task<PrivateFileData> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PrivateFileData>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<PrivateFileData> CreateFileAsync(PrivateFileData privateFileDataCreateRequest, CancellationToken cancellationToken = default);
    Task<IEnumerable<PrivateFileData>> CreateFilesAsync(IEnumerable<PrivateFileData> fileDataCreateRequests, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}