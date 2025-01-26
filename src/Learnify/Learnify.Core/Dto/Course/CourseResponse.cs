using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Dto.File;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

public class CourseResponse
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PrimaryLanguage { get; set; }
    public PrivateFileDataResponse Photo { get; set; }
    public PrivateFileDataResponse Video { get; set; }
    public IEnumerable<ParagraphResponse> Paragraphs { get; set; }
}