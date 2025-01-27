namespace Learnify.Core.Dto.Params;

public class CourseParams
{
    public PagedListParams PagedListParams { get; set; }
    public OrderByParams OrderByParams { get; set; }
    public string Search { get; set; }
    public int? AuthorId { get; set; }
}