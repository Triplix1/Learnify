﻿namespace Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

/// <summary>
/// Unit of work for profile app
/// </summary>
public interface IPsqUnitOfWork
{
    /// <summary>
    /// Get value of ProfileRepository
    /// </summary>
    IUserRepository UserRepository { get; }
    
    /// <summary>
    /// Get value of ProfileRepository
    /// </summary>
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    /// <summary>
    /// Get value of CourseRatingsRepository
    /// </summary>
    ICourseRatingsRepository CourseRatingsRepository { get; }
    
    /// <summary>
    /// Get value of CourseRepository
    /// </summary>
    ICourseRepository CourseRepository { get; }
    
    /// <summary>
    /// Saves changes made on db
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
    
}