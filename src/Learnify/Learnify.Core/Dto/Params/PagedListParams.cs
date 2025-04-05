namespace Learnify.Core.Dto.Params;

public class PagedListParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}