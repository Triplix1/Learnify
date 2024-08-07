using System.Linq.Expressions;

namespace Learnify.Core.Specification;

public abstract class Specification<T>
{
    // Reference to the paragraph: https://www.linkedin.com/pulse/specification-pattern-c-collins-ezerioha/
    public abstract Expression<Func<T, bool>> GetExpression();
    
    public bool IsSatisfiedBy(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof (obj));
        return GetExpression().Compile()(obj);
    }
    
    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
    {
        return new AndSpecification<T>(left, right);
    }

    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
    {
        return new OrSpecification<T>(left, right);
    }

    public static Specification<T> operator !(Specification<T> spec)
    {
        return new NotSpecification<T>(spec);
    }
}