using Learnify.Core.Dto.Blob;

namespace Learnify.Core.Dto.File;

public class PrivateFileBlobCreateRequest
{
    public BlobDto BlobDto { get; set; }
    public string ContentType { get; set; }
    public int LessonId { get; set; }
}