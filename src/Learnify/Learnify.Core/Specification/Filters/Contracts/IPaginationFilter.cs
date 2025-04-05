using Learnify.Core.Dto;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.Specification.Filters.Contracts;

public interface IPaginationFilter
{
    PagedListParams PagedListParams { get; set; }
}