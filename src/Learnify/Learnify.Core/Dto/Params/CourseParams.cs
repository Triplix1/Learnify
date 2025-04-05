namespace Learnify.Core.Dto.Params;

public class CourseParams: DefaultListParams
{
    public string Search { get; set; }
    public int? AuthorId { get; set; }
}