using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Learnify.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// RefreshTokenRepository
/// </summary>
public class RefreshTokenRepository : BasePsqRepository<RefreshToken, int>, IRefreshTokenRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="RefreshTokenRepository"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetByJwtAsync(string jwt, CancellationToken cancellationToken = default)
    {
        return await Context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Jwt == jwt,
            cancellationToken: cancellationToken);
    }
}