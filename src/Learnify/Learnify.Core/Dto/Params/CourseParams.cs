namespace Learnify.Core.Dto.Params;

public class CourseParams: PaginatedParams
{
    public OrderByParams OrderByParams { get; set; }
    public string Search { get; set; }
}