using Learnify.Core.Dto;

namespace Learnify.Core.Specification.Filters.Contracts;

public interface IPaginationFilter
{
    PagedListParams PagedListParams { get; set; }
}