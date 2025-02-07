using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.Dto.Course;

public class CourseStudyResponse
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public IEnumerable<ParagraphResponse> Paragraphs { get; set; }
}