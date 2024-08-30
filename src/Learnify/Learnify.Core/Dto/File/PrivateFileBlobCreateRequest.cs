namespace Learnify.Core.Dto.File;

public class PrivateFileBlobCreateRequest
{
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
    public int LessonId { get; set; }
}