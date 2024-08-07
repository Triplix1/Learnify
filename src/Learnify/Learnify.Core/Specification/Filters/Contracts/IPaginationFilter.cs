namespace Learnify.Core.Specification.Filters.Contracts;

public interface IPaginationFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}