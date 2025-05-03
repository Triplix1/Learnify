using Learnify.Core.Dto.File;

namespace Learnify.Core.ServiceContracts;

public interface IFileService
{
    Task<FileStreamResponse> GetFileStreamById(int id, int? userId, CancellationToken cancellationToken = default);

    Task<PrivateFileDataResponse> CreateAsync(PrivateFileBlobCreateRequest privateFileBlobCreateRequest,
        bool isPublic, int userId, CancellationToken cancellationToken = default);
}