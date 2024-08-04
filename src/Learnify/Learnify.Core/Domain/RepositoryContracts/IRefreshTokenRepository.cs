using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// RefreshTokenRepository
/// </summary>
public interface IRefreshTokenRepository: IBaseRepository<RefreshToken, int>
{
    /// <summary>
    /// Returns refresh token by access token
    /// </summary>
    /// <param name="jwt"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetByJwtAsync(string jwt);
}