using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Specification;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IViewsRepository
{
    IEnumerable<View> GetFilteredData(ViewSpecificationFilter specificationFilter);
}