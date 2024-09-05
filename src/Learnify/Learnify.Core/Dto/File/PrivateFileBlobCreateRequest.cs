using Microsoft.AspNetCore.Http;

namespace Learnify.Core.Dto.File;

public class PrivateFileBlobCreateRequest
{
    public IFormFile Content { get; set; }
    public string ContentType { get; set; }
    public int? CourseId { get; set; }
}