namespace Learnify.Core.Dto.File;

public class PrivateFileDataCreateRequest
{
    public string ContentType { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
    public string LessonId { get; set; }
}