using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Dto.File;

namespace Learnify.Core.Dto.Course;

public class CourseMainInfoResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PrimaryLanguage { get; set; }
    public bool UserHasBoughtThisCourse { get; set; }
    public PrivateFileDataResponse Video { get; set; }
    public IEnumerable<ParagraphResponse> Paragraphs { get; set; }
}