namespace ClassLibrary1.Blob;

public class BlobAddRequest
{
    public string Name { get; set; }
    public string ContainerName { get; set; }
    public byte[]? Content { get; set; }
}