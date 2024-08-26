using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IViewRepository
{
    IEnumerable<View> GetFilteredData(ViewSpecificationFilter specificationFilter);
}