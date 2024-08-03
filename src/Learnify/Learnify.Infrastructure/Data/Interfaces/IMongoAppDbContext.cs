using Learnify.Core.Domain.Entities;
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
}