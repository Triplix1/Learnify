using System.Linq.Expressions;

namespace Learnify.Core.Specification.Base;

public class TrueSpecification<T>: Specification<T>
{
    public override Expression<Func<T, bool>> GetExpression()
    {
        return x => true;
    }
}