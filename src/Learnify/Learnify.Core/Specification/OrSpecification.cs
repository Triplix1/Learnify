using System.Linq.Expressions;
using Learnify.Core.Extensions;

namespace Learnify.Core.Specification;

public class OrSpecification<T>: Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left ?? throw new ArgumentNullException(nameof (left));
        _right = right ?? throw new ArgumentNullException(nameof (right));
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return _left.GetExpression().OrElse(_right.GetExpression());
    }
}