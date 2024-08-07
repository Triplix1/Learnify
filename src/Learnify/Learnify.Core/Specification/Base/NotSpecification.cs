using System.Linq.Expressions;
using Learnify.Core.Extensions;

namespace Learnify.Core.Specification;

public class NotSpecification<T>: Specification<T>
{
    private readonly Specification<T> _specification;

    public NotSpecification(Specification<T> spec)
    {
        _specification = spec ?? throw new ArgumentNullException(nameof (spec));
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return _specification.GetExpression().Not();
    } 
}