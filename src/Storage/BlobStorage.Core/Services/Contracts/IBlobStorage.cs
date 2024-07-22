using BlobStorage.Core.Models;
using Grpc.Core;

namespace BlobStorage.Core.Services.Contracts;

public interface IBlobStorage
{
    Task<BlobResponse> UploadAsync(BlobDto blobDto, ServerCallContext context = null);
    Task<BlobResponse> UpdateAsync(BlobDto blobDto, ServerCallContext context = null);
    Task<bool> DeleteAsync(string containerName, string blobId, ServerCallContext context = null);
    Task<string> GetFileUrlAsync(string containerName, string blobId, ServerCallContext context = null);
}