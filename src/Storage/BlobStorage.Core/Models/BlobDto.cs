namespace BlobStorage.Core.Models;

public class BlobDto
{
    public string Name { get; set; }
    public string ContainerName { get; set; }
    public string ContentType { get; set; } = "octet-stream";
    public byte[]? Content { get; set; }
}