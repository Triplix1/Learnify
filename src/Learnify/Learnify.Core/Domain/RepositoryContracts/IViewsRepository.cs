using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IViewsRepository
{
    IEnumerable<View> GetFilteredData(ViewSpecificationFilter specificationFilter);
}