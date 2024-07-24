using General.RepositoryInterfaces;

namespace Profile.Core.Domain.RepositoryContracts;

/// <summary>
/// Unit of work for profile app
/// </summary>
public interface IUnitOfWork: IBaseUnitOfWork
{
    /// <summary>
    /// Get value of ProfileRepository
    /// </summary>
    IProfileRepository ProfileRepository { get; }
}