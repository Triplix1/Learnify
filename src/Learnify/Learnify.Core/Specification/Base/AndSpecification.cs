using System.Linq.Expressions;
using Learnify.Core.Extensions;

namespace Learnify.Core.Specification.Base;

public class AndSpecification<T>: Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        this._left = left ?? throw new ArgumentNullException(nameof (left));
        this._right = right ?? throw new ArgumentNullException(nameof (right));
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return _left.GetExpression().AndAlso(_right.GetExpression());
    }
}