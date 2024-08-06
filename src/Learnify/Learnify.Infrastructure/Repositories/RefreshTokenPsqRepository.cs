using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// RefreshTokenRepository
/// </summary>
public class RefreshTokenPsqRepository: BasePsqRepository<RefreshToken, int>, IRefreshTokenRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="RefreshTokenPsqRepository"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public RefreshTokenPsqRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetByJwtAsync(string jwt)
    {
        return await Context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Jwt == jwt);
    }
}