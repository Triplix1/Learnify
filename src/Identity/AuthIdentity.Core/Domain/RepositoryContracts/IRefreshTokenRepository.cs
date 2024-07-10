using AuthIdentity.Core.Domain.Entities;

namespace AuthIdentity.Core.Domain.RepositoryContracts;

public interface IRefreshTokenRepository: IBaseRepository<RefreshToken>
{
    Task<RefreshToken?> GetByJwtAsync(string jwt);
}