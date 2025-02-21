using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.ManagerContracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto, bool isPrivate = false,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string containerName, string blobId, CancellationToken cancellationToken = default);
    Task<string> GetFileUrlAsync(string containerName, string blobId, CancellationToken cancellationToken = default);

    Task<FileStreamResponse> GetBlobStreamAsync(GetBlobParams getBlobParams,
        CancellationToken cancellationToken = default);

    Task<string> GetHlsManifestUrl(string containerName, string blobName,
        CancellationToken cancellationToken = default);
}