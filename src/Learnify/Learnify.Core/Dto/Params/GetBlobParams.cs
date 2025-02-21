namespace Learnify.Core.Dto.Params;

public class GetBlobParams
{
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
    public string ContentType { get; set; } = "application/octet-stream";
}