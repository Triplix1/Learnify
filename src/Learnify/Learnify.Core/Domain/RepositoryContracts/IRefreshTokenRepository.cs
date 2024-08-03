using Learnify.Core.Domain.Entities;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// RefreshTokenRepository
/// </summary>
public interface IRefreshTokenRepository: IBaseRepository<RefreshToken>
{
    /// <summary>
    /// Returns refresh token by access token
    /// </summary>
    /// <param name="jwt"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetByJwtAsync(string jwt);
}