using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.NoSql;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Data.Interfaces;

/// <summary>
/// Mongo application db context
/// </summary>
public interface IMongoAppDbContext
{
    /// <summary>
    /// Collection of <see cref="View"/>
    /// </summary>
    IMongoCollection<View> Views { get; }
    
    /// <summary>
    /// Collection of <see cref="Lesson"/>
    /// </summary>
    IMongoCollection<CourseLessonContent> Lessons { get; }
    
    /// <summary>
    /// Collection of <see cref="Course"/>
    /// </summary>
    IMongoCollection<Course> Courses { get; }

    /// <summary>
    /// Gets collection for specified type by specified name
    /// </summary>
    /// <param name="name">Collection name</param>
    /// <typeparam name="T">type</typeparam>
    /// <returns>Collection for specified type by specified name</returns>
    IMongoCollection<T> GetCollection<T>(string name);
}