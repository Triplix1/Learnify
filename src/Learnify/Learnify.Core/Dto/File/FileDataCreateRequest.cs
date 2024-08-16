namespace Learnify.Core.Dto.File;

public class FileDataCreateRequest
{
    public string ContentType { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
}