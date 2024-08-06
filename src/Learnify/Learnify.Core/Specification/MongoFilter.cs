namespace Learnify.Core.Specification;

public class MongoFilter<T>: IPaginationFilter<T>
{
    public Specification<T> Specification { get; set; }
    public SpecificationPaginator Pagination { get; set; }
}