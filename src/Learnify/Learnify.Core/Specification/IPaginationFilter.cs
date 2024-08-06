namespace Learnify.Core.Specification;

public interface IPaginationFilter<T>: IFilter<T>
{
    SpecificationPaginator Pagination { get; set; }
}