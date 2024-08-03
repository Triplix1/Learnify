using Learnify.Core.Dto;

namespace Learnify.Core.ManagerContracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto);
    Task<bool> DeleteAsync(string containerName, string blobId);
    Task<string> GetFileUrlAsync(string containerName, string blobId);
}