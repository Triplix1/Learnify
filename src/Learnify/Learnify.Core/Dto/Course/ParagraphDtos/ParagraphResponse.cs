namespace Learnify.Core.Dto.Course.ParagraphDtos;

public class ParagraphResponse
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public bool IsPublished { get; set; }
}