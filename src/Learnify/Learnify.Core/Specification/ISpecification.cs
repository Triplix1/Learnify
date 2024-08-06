using System.Linq.Expressions;

namespace Learnify.Core.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Expression { get; }
    Func<T, bool> ToPredicate();
    bool IsSatisfiedBy(T obj);
}