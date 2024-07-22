using General.RepositoryInterfaces;
using Profile.Core.Domain.Entities;

namespace Profile.Core.Domain.RepositoryContracts;

/// <summary>
/// Repository for working with db entity of profile
/// </summary>
public interface IProfileRepository : IAsyncGettableRepository<User>, IAsyncCreatableRepository<User>, IAsyncDeletableRepository<User>, IAsyncGettableByIdRepository<User, string>, IAsyncUpdatableRepository<User>
{
}