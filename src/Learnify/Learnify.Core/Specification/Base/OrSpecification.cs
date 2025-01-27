using System.Linq.Expressions;
using Learnify.Core.Extensions;

namespace Learnify.Core.Specification.Base;

public class OrSpecification<T>: Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left ?? new TrueSpecification<T>();
        _right = right ?? new TrueSpecification<T>();
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return _left.GetExpression().OrElse(_right.GetExpression());
    }
}