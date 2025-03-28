﻿using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.Base;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// RefreshTokenRepository
/// </summary>
public interface IRefreshTokenRepository: IBasePsqRepository<RefreshToken, int>
{
    /// <summary>
    /// Returns refresh token by access token
    /// </summary>
    /// <param name="jwt"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetByJwtAsync(string jwt, CancellationToken cancellationToken = default);
}