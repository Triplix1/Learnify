using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class BlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<BlobResponse> UploadAsync(BlobDto blobDto, CancellationToken cancellationToken = default)
    {
        return await UploadSingleBlobAsync(blobDto, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string containerName, string blobId,
        CancellationToken cancellationToken = default)
    {
        var blobClient = await GetBlobClientInternalAsync(containerName, blobId, cancellationToken);

        return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<BlobStreamResponse> GetBlobStreamAsync(GetBlobParams getBlobParams,
        CancellationToken cancellationToken = default)
    {
        var blobClient =
            await GetBlobClientInternalAsync(getBlobParams.ContainerName, getBlobParams.BlobName, cancellationToken);

        if (!await blobClient.ExistsAsync(cancellationToken))
            throw new FileNotFoundException(
                $"Blob with name:{getBlobParams.BlobName} does not exist in container:{getBlobParams.ContainerName}.");

        var blobStream = await blobClient.OpenReadAsync(cancellationToken: cancellationToken);

        var response = new BlobStreamResponse()
        {
            Stream = blobStream,
            ContentType = getBlobParams.ContentType
        };

        return response;
    }

    private async Task<BlobClient> GetBlobClientInternalAsync(string containerName, string blobName,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        return containerClient.GetBlobClient(blobName);
    }

    private async Task<BlobResponse> UploadSingleBlobAsync(BlobDto blobDto, CancellationToken cancellationToken)
    {
        var blobClient = await GetBlobClientInternalAsync(blobDto.ContainerName, blobDto.Name, cancellationToken);

        if (await blobClient.ExistsAsync(cancellationToken))
            throw new InvalidOperationException($"Blob with id:{blobDto.Name} already exists.");

        if (blobDto.Content is null)
            throw new ArgumentNullException(nameof(blobDto.Content));

        var blobHttpHeaders = new BlobHttpHeaders { ContentType = blobDto.ContentType };
        var blobUploadOptions = new BlobUploadOptions { HttpHeaders = blobHttpHeaders };

        await blobClient.UploadAsync(blobDto.Content, blobUploadOptions, cancellationToken);

        return new BlobResponse()
        {
            Name = blobClient.Name,
            ContainerName = blobClient.BlobContainerName,
            ContentType = (await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken)).Value.ContentType
        };
    }

    // private async Task<BlobResponse> UploadHlsBlobsAsync(BlobDto blobDto, CancellationToken cancellationToken)
    // {
    //     if (blobDto.Content == null)
    //         throw new ArgumentNullException(nameof(blobDto.Content));
    //
    //     // Step 1: Save uploaded video temporarily to disk
    //     var tempFilePath = Path.GetTempFileName();
    //     await using (var fileStream = File.Create(tempFilePath))
    //     {
    //         await blobDto.Content.CopyToAsync(fileStream, cancellationToken);
    //     }
    //
    //     // Step 2: Convert MP4 -> HLS (generate .m3u8 + .ts files)
    //     var hlsOutputFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    //     Directory.CreateDirectory(hlsOutputFolder);
    //
    //     var ffmpegArguments =
    //         $"-i \"{tempFilePath}\" -codec: copy -start_number 0 -hls_time 10 -hls_list_size 0 -f hls \"{Path.Combine(hlsOutputFolder, "index.m3u8")}\"";
    //
    //     var process = new Process
    //     {
    //         StartInfo = new ProcessStartInfo
    //         {
    //             FileName = "ffmpeg",
    //             Arguments = ffmpegArguments,
    //             RedirectStandardOutput = true,
    //             RedirectStandardError = true,
    //             UseShellExecute = false,
    //             CreateNoWindow = true
    //         }
    //     };
    //     process.Start();
    //     await process.WaitForExitAsync(cancellationToken);
    //
    //     if (process.ExitCode != 0)
    //     {
    //         var error = await process.StandardError.ReadToEndAsync(cancellationToken);
    //         throw new Exception($"FFmpeg HLS conversion failed: {error}");
    //     }
    //
    //     // Step 3: Upload all HLS files (.ts and .m3u8) to Azure Blob Storage
    //
    //     foreach (var filePath in Directory.GetFiles(hlsOutputFolder))
    //     {
    //         var fileName = $"{Path.GetFileNameWithoutExtension(blobDto.Name)}/{Path.GetFileName(filePath)}";
    //         var blobClient = await GetBlobClientInternalAsync(blobDto.ContainerName, fileName, cancellationToken);
    //
    //         await using var uploadStream = File.OpenRead(filePath);
    //         var contentType = filePath.EndsWith(".m3u8") ? "application/vnd.apple.mpegurl" : "video/MP2T";
    //
    //         await blobClient.UploadAsync(uploadStream, new BlobHttpHeaders { ContentType = contentType },
    //             cancellationToken: cancellationToken);
    //     }
    //
    //     // Step 4: Generate SAS URL for the master playlist (index.m3u8)
    //     var playlistBlobClient =
    //         await GetBlobClientInternalAsync(blobDto.ContainerName,
    //             $"{Path.GetFileNameWithoutExtension(blobDto.Name)}/index.m3u8", cancellationToken);
    //
    //     // Step 5: Cleanup temp files
    //     File.Delete(tempFilePath);
    //     Directory.Delete(hlsOutputFolder, true);
    //
    //     return new BlobResponse()
    //     {
    //         Name = playlistBlobClient.Name,
    //         ContainerName = playlistBlobClient.BlobContainerName,
    //         ContentType = "application/vnd.apple.mpegurl"
    //     };
    // }
}