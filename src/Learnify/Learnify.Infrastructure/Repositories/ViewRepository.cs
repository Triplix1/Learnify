using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Specification.Filters;

namespace Learnify.Infrastructure.Repositories;

public class ViewRepository: IViewRepository
{
    public IEnumerable<View> GetFilteredData(ViewSpecificationFilter specificationFilter)
    {
        throw new NotImplementedException();
    }
}