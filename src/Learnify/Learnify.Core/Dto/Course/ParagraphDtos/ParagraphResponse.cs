using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Dto.Course.ParagraphDtos;

public class ParagraphResponse
{
    public string Name { get; set; }
    public IList<LessonListResponse> Lessons { get; set; }
}