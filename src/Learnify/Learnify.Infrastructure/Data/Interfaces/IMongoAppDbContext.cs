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
    /// Collection of <see cref="Lessons"/>
    /// </summary>
    IMongoCollection<CourseLessonContent> Lessons { get; }
}