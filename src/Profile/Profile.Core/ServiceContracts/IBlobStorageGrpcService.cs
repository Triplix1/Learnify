using BlobStorage.Grpc.Protos;

namespace Profile.Core.ServiceContracts;

/// <summary>
/// Grpc service 
/// </summary>
public interface IBlobStorageGrpcService
{
    /// <summary>
    /// Deletes blob from storage
    /// </summary>
    /// <param name="containerName">Container name</param>
    /// <param name="blobId">Blob name</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(string containerName, string blobId);
    
    /// <summary>
    /// Upload file to storage
    /// </summary>
    /// <param name="blobDto"><see cref="BlobDto"/></param>
    /// <returns></returns>
    Task<BlobResponse> UploadAsync(BlobDto blobDto);
    
    /// <summary>
    /// Updates blob 
    /// </summary>
    /// <param name="blobDto"><see cref="BlobDto"/></param>
    /// <returns></returns>
    Task<BlobResponse> UpdateAsync(BlobDto blobDto);
}