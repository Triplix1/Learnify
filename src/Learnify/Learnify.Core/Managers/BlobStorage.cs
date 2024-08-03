using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Learnify.Core.Dto;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class BlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<BlobResponse> UploadAsync(BlobDto blobDto)
    {
        var blobClient = await GetBlobClientInternalAsync(blobDto.ContainerName, blobDto.Name);

        if (await blobClient.ExistsAsync())
        {
            throw new InvalidOperationException($"BlobDto with id:{blobDto.Name} already exists.");
        }

        await blobClient.UploadAsync(new BinaryData(blobDto.Content ?? new byte[] { }));
        
        BlobSasBuilder sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobDto.ContainerName,
            BlobName = blobDto.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

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

        return await blobClient.DeleteIfExistsAsync();
    }
    
    public async Task<string> GetFileUrlAsync(string containerName, string blobId)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobId);
        
        if (!await blobClient.ExistsAsync())
        {
            throw new InvalidOperationException($"Blob with id:{blobId} does not exist.");
        }

        return blobClient.Uri.AbsoluteUri;
    }
    
    public async Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobName);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob with name:{blobName} does not exist in container:{containerName}.");
        }

        var blobStream = await blobClient.OpenReadAsync();
        return blobStream;
    }

    private async Task<BlobClient> GetBlobClientInternalAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        
        return containerClient.GetBlobClient(blobName);
    }
}