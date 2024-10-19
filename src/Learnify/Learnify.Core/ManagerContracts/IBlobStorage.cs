using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;

namespace Learnify.Core.ManagerContracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto, bool isPrivate = false,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string containerName, string blobId, CancellationToken cancellationToken = default);
    Task<string> GetFileUrlAsync(string containerName, string blobId, CancellationToken cancellationToken = default);

    Task<FileStreamResponse> GetBlobStreamAsync(string containerName, string blobName,
        CancellationToken cancellationToken = default);
}