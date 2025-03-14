﻿using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.Base;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// UserRepository
/// </summary>
public interface IUserRepository : IBasePsqRepository<User, int>
{
    /// <summary>
    /// Returns user by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns></returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns user by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns></returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}