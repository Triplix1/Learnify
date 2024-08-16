namespace Learnify.Core.Dto.File;

public class FileDataResponse
{
    public int Id { get; set; }
    public string ContentType { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
}