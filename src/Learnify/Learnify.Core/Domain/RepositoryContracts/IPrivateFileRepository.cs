using Learnify.Core.Dto.File;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IPrivateFileRepository
{
    Task<PrivateFileBlobInfoResponse> GetByIdAsync(int id);
    Task<IEnumerable<PrivateFileBlobInfoResponse>> GetByIdsAsync(IEnumerable<int> ids);
    Task<PrivateFileDataResponse> CreateFileAsync(PrivateFileDataCreateRequest privateFileDataCreateRequest);
    Task<IEnumerable<PrivateFileDataResponse>> CreateFilesAsync(IEnumerable<PrivateFileDataCreateRequest> fileDataCreateRequests);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
}