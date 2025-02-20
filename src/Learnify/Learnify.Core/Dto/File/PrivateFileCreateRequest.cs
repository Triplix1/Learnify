namespace Learnify.Core.Dto.File;

public class PrivateFileCreateRequest
{
    public string ContentType { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
    public int? CourseId { get; set; }
}