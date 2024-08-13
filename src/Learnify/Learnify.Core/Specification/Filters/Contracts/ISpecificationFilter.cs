using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Filters.Contracts;

public interface ISpecificationFilter<T>
{
    Specification<T> Specification { get; set; }
}