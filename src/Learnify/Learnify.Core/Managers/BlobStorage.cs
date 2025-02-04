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

    public async Task<BlobResponse> UploadAsync(BlobDto blobDto, bool isPrivate = false,
        CancellationToken cancellationToken = default)
    {
        var blobClient = await GetBlobClientInternalAsync(blobDto.ContainerName, blobDto.Name, cancellationToken);

        if (await blobClient.ExistsAsync(cancellationToken))
            throw new InvalidOperationException($"BlobDto with id:{blobDto.Name} already exists.");

        if (blobDto.Content is null)
            throw new ArgumentNullException(nameof(blobDto.Content), "Content to upload into storage cannot be null");

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = blobDto.ContentType // Use the ContentType from BlobDto
        };

        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        };

        await blobClient.UploadAsync(blobDto.Content, blobUploadOptions, cancellationToken);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobDto.ContainerName,
            BlobName = blobDto.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(3)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);

        // await _redisCacheManager.SetCachedDataAsync(blobClient.BlobContainerName + blobClient.Name, sasUri.ToString(), new TimeSpan(3,0,0));

        return new BlobResponse()
        {
            Name = blobClient.Name,
            ContainerName = blobClient.BlobContainerName,
            Url = sasUri.ToString(),
            ContentType = (await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken)).Value.ContentType
        };
    }

    public async Task<bool> DeleteAsync(string containerName, string blobId,
        CancellationToken cancellationToken = default)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobId, cancellationToken);

        await _redisCacheManager.RemoveCachedDataAsync(containerName + blobId, cancellationToken);

        return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<string> GetFileUrlAsync(string containerName, string blobId,
        CancellationToken cancellationToken = default)
    {
        var url = await _redisCacheManager.GetCachedDataAsync<string>(containerName + blobId, cancellationToken);

        if (url is null)
        {
            var blobClient = await GetBlobClientInternalAsync(containerName, blobId, cancellationToken);

            if (!await blobClient.ExistsAsync(cancellationToken))
                throw new InvalidOperationException($"Blob with id:{blobId} does not exist.");

            url = blobClient.Uri.AbsoluteUri;
        }

        return url;
    }

    public async Task<FileStreamResponse> GetBlobStreamAsync(string containerName, string blobName,
        CancellationToken cancellationToken = default)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobName, cancellationToken);

        if (!await blobClient.ExistsAsync(cancellationToken))
            throw new FileNotFoundException($"Blob with name:{blobName} does not exist in container:{containerName}.");

        var blobProperties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

        var blobStream = await blobClient.OpenReadAsync(cancellationToken: cancellationToken);

        var response = new FileStreamResponse()
        {
            Stream = blobStream,
            ContentType = blobProperties.Value.ContentType
        };

        return response;
    }
    
    public async Task<string> GetHlsManifestUrl(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobName.Replace(".mp4", "/manifest.m3u8"), cancellationToken);
    
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTime.UtcNow.AddMinutes(10) // Link expires in 10 minutes
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
    
        var sasUrl = blobClient.GenerateSasUri(sasBuilder).Query;
        return sasUrl;
    }

    private async Task<BlobClient> GetBlobClientInternalAsync(string containerName, string blobName,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        return containerClient.GetBlobClient(blobName);
    }
}