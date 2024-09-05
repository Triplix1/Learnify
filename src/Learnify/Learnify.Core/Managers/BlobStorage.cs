using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class BlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IRedisCacheManager _redisCacheManager;

    public BlobStorage(BlobServiceClient blobServiceClient, IRedisCacheManager redisCacheManager)
    {
        _blobServiceClient = blobServiceClient;
        _redisCacheManager = redisCacheManager;
    }

    public async Task<BlobResponse> UploadAsync(BlobDto blobDto, bool isPrivate = false)
    {
        var blobClient = await GetBlobClientInternalAsync(blobDto.ContainerName, blobDto.Name);

        if (await blobClient.ExistsAsync())
        {
            throw new InvalidOperationException($"BlobDto with id:{blobDto.Name} already exists.");
        }
        
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = blobDto.ContentType // Use the ContentType from BlobDto
        };
        
        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        };
        
        await blobClient.UploadAsync(new BinaryData(blobDto.Content ?? new byte[] { }), blobUploadOptions);
        
        BlobSasBuilder sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobDto.ContainerName,
            BlobName = blobDto.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(3)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
        
        await _redisCacheManager.SetCachedDataAsync(blobClient.BlobContainerName + blobClient.Name, sasUri.ToString(), new TimeSpan(3,0,0));
        
        return new BlobResponse()
        {
            Name = blobClient.Name,
            ContainerName = blobClient.BlobContainerName,
            Url = sasUri.ToString(),
            ContentType = (await blobClient.GetPropertiesAsync()).Value.ContentType
        };
    }
    
    public async Task<bool> DeleteAsync(string containerName, string blobId)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobId);

        await _redisCacheManager.RemoveCachedDataAsync(containerName + blobId);

        return await blobClient.DeleteIfExistsAsync();
    }
    
    public async Task<string> GetFileUrlAsync(string containerName, string blobId)
    {
        var url = await _redisCacheManager.GetCachedDataAsync<string>(containerName + blobId);

        if (url is null)
        {
            var blobClient = await GetBlobClientInternalAsync(containerName, blobId);
        
            if (!await blobClient.ExistsAsync())
            {
                throw new InvalidOperationException($"Blob with id:{blobId} does not exist.");
            }

            url = blobClient.Uri.AbsoluteUri;
        }
        
        return url;
    }
    
    public async Task<FileStreamResponse> GetBlobStreamAsync(string containerName, string blobName)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobName);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob with name:{blobName} does not exist in container:{containerName}.");
        }

        var blobProperties = await blobClient.GetPropertiesAsync();
        
        var blobStream = await blobClient.OpenReadAsync();

        var response = new FileStreamResponse()
        {
            Stream = blobStream,
            ContentType = blobProperties.Value.ContentType
        };

        return response;
    }
    
    private async Task<BlobClient> GetBlobClientInternalAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        
        await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

        await containerClient.CreateIfNotExistsAsync();
        
        return containerClient.GetBlobClient(blobName);
    }
}