using System.Linq.Expressions;

namespace Learnify.Core.Specification;

public class EfFilter<T>: IPaginationFilter<T>
{
    public Specification<T> Specification { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; set; }
    public SpecificationPaginator Pagination { get; set; }
}