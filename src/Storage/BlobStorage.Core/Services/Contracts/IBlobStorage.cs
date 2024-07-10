using BlobStorage.Core.Models;

namespace BlobStorage.Core.Services.Contracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto);
    Task<BlobResponse> UpdateAsync(BlobDto blobDto);
    Task<bool> DeleteAsync(string containerName, string blobId);
    Task<string> GetFileUrlAsync(string containerName, string blobId);
}