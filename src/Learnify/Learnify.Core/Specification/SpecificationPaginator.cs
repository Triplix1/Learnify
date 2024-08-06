namespace Learnify.Core.Specification;

public class SpecificationPaginator
{
    public SpecificationPaginator(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
    public int Skip { get; }
    public int Take { get; }
}