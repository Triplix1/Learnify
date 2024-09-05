using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;

namespace Learnify.Core.ManagerContracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto, bool isPrivate = false);
    Task<bool> DeleteAsync(string containerName, string blobId);
    Task<string> GetFileUrlAsync(string containerName, string blobId);
    Task<FileStreamResponse> GetBlobStreamAsync(string containerName, string blobName); 
}