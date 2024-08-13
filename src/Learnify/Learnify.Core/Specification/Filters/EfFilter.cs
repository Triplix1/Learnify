using System.Linq.Expressions;
using Learnify.Core.Dto.Params;
using Learnify.Core.Specification.Base;
using Learnify.Core.Specification.Filters.Contracts;

namespace Learnify.Core.Specification.Filters;

public class EfFilter<T>: IBaseEntityFilter<T>
{
    public Specification<T> Specification { get; set; }
    public OrderByParams OrderByParams { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}