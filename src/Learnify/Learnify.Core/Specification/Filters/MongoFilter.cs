using Learnify.Core.Specification.Filters.Contracts;

namespace Learnify.Core.Specification;

public class MongoFilter<T>: IBaseEntityFilter<T>
{
    public Specification<T> Specification { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}