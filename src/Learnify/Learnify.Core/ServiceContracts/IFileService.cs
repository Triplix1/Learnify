using Learnify.Core.Dto.File;

namespace Learnify.Core.ServiceContracts;

public interface IFileService
{
    Task<FileStreamResponse> GetFileStreamById(int id, int userId);
    Task<PrivateFileDataResponse> CreateAsync(PrivateFileBlobCreateRequest privateFileBlobCreateRequest);
}