using Learnify.Core.Dto.File;

namespace Learnify.Core.ManagerContracts;

public interface IPrivateFileManager
{
    Task<PrivateFileDataResponse> CreateAsync(PrivateFileBlobCreateRequest privateFileBlobCreateRequest);
    Task DeleteAsync(int id);
    Task DeleteRangeAsync(IEnumerable<int> ids);
}