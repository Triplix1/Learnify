using Learnify.Core.Domain.Entities.NoSql;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Data.Interfaces;

/// <summary>
/// Mongo application db context
/// </summary>
public interface IMongoAppDbContext
{
    /// <summary>
    /// Collection of <see cref="Lesson"/>
    /// </summary>
    IMongoCollection<Lesson> Lessons { get; }

    /// <summary>
    /// Gets collection for specified type by specified name
    /// </summary>
    /// <param name="name">Collection name</param>
    /// <typeparam name="T">type</typeparam>
    /// <returns>Collection for specified type by specified name</returns>
    IMongoCollection<T> GetCollection<T>(string name);
}