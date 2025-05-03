using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.ManagerContracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string containerName, string blobId, CancellationToken cancellationToken = default);

    Task<BlobStreamResponse> GetBlobStreamAsync(GetBlobParams getBlobParams,
        CancellationToken cancellationToken = default);
}