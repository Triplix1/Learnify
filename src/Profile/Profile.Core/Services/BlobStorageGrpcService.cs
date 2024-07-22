using BlobStorage.Grpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Profile.Core.ServiceContracts;

namespace Profile.Core.Services;

/// <inheritdoc />
public class BlobStorageGrpcService: IBlobStorageGrpcService
{
    private readonly BlobStorage.Grpc.Protos.BlobStorage.BlobStorageClient _client;
    private readonly ILogger<BlobStorageGrpcService> _logger;

    /// <summary>
    /// Initializes an instance of <see cref="BlobStorageGrpcService"/>
    /// </summary>
    /// <param name="client"><see cref="BlobStorage.Grpc.Protos.BlobStorage.BlobStorageClient"/></param>
    /// <param name="logger"><see cref="ILogger"/></param>
    public BlobStorageGrpcService(BlobStorage.Grpc.Protos.BlobStorage.BlobStorageClient client,
        ILogger<BlobStorageGrpcService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string containerName, string blobId)
    {
        var deleteRequest = new DeleteRequest()
        {
            BlobId = blobId,
            ContainerName = containerName
        };
        
        try
        {
            var response = await _client.DeleteAsync(deleteRequest);

            return response.Success;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "ERROR - Parameters: {@parameters}", deleteRequest);
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<BlobResponse> UploadAsync(BlobDto blobDto)
    {
        var response = null as BlobResponse;
        try
        {
            response = await _client.UploadAsync(blobDto);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "ERROR - Parameters: {@parameters}", blobDto);
        }
        
        return response;
    }

    /// <inheritdoc />
    public async Task<BlobResponse> UpdateAsync(BlobDto blobDto)
    {
        var response = null as BlobResponse;

        try
        {
            response = await _client.UpdateAsync(blobDto);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "ERROR - Parameters: {@parameters}", blobDto);
        }
        
        return response;
    }
}