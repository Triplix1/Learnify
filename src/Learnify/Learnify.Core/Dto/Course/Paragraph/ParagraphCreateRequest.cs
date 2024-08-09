namespace Learnify.Core.Dto.Course.Paragraph;

public class ParagraphCreateRequest
{
    public string Name { get; set; }
    public IList<Lesson> Lessons { get; set; }
}