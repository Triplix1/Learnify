using Learnify.Core.Specification.Base;
using Learnify.Core.Specification.Filters.Contracts;

namespace Learnify.Core.Specification.Filters;

public class MongoFilter<T>: IBaseEntityFilter<T>
{
    public Specification<T> Specification { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}