using Learnify.Core.Dto;
using Learnify.Core.Dto.Params;
using Learnify.Core.Specification.Base;
using Learnify.Core.Specification.Filters.Contracts;

namespace Learnify.Core.Specification.Filters;

public class MongoFilter<T>: IBaseEntityFilter<T>
{
    public Specification<T> Specification { get; set; }
    public PagedListParams PagedListParams { get; set; }
}