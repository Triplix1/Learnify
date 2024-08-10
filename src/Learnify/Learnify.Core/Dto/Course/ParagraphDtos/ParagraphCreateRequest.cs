using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Dto.Course.ParagraphDtos;

public class ParagraphCreateRequest
{
    public string Name { get; set; }
    public IList<LessonCreateRequest> Lessons { get; set; }
}