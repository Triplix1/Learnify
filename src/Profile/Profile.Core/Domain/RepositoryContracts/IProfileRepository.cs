using General.RepositoryInterfaces;
using Profile.Core.Domain.Entities;

namespace Profile.Core.Domain.RepositoryContracts;

public interface IProfileRepository : IAsyncGettableRepository<User>, IAsyncCreatableRepository<User>, IAsyncDeletableRepository<User>, IAsyncGettableByIdRepository<User, string>, IAsyncUpdatebaleRepository<User>
{
}