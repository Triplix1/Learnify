using System.Linq.Expressions;
using Learnify.Core.Extensions;

namespace Learnify.Core.Specification.Base;

public class AndSpecification<T>: Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        this._left = left ?? new TrueSpecification<T>();
        this._right = right ?? new TrueSpecification<T>();
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return _left.GetExpression().AndAlso(_right.GetExpression());
    }
}