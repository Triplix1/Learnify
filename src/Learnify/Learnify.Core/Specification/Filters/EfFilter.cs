using System.Linq.Expressions;
using Learnify.Core.Specification.Filters.Contracts;

namespace Learnify.Core.Specification;

public class EfFilter<T>: IBaseEntetyFilter<T>
{
    public Specification<T> Specification { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}