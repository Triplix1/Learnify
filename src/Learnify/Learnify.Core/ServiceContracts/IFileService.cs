using Learnify.Core.Dto.File;

namespace Learnify.Core.ServiceContracts;

public interface IFileService
{
    Task<Stream> GetBlobStreamByIdAsync(int id);
    Task<PrivateFileDataResponse> CreateAsync(PrivateFileBlobCreateRequest privateFileBlobCreateRequest);
    Task DeleteAsync(int id);
    Task DeleteRangeAsync(IEnumerable<int> ids);
}