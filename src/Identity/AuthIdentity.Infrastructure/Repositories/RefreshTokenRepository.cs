using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace AuthIdentity.Infrastructure.Repositories;

public class RefreshTokenRepository: BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IdentityDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByJwtAsync(string jwt)
    {
        return await Context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Jwt == jwt);
    }
}