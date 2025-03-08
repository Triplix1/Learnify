namespace Learnify.Core.Dto.File;

public class PrivateFileDataBlobResponse
{
    public int Id { get; set; }
    public string ContentType { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
    public int? CourseId { get; set; }
}