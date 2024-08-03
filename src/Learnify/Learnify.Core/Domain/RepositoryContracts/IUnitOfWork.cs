namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Unit of work for profile app
/// </summary>
public interface IUnitOfWork: IBaseUnitOfWork
{
    /// <summary>
    /// Get value of ProfileRepository
    /// </summary>
    IUserRepository UserRepository { get; }
    
    /// <summary>
    /// Get value of ProfileRepository
    /// </summary>
    IRefreshTokenRepository RefreshTokenRepository { get; }
}