namespace Learnify.Core.Specification;

public interface IFilter<T>
{
    Specification<T> Specification { get; set; }
}