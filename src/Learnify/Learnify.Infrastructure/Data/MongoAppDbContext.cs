using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Data;

/// <inheritdoc />
public class MongoAppDbContext: IMongoAppDbContext
{
    private readonly IMongoDatabase _database;
    
    /// <summary>
    /// Initializes a new instance of <see cref="MongoAppDbContext"/>
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    public MongoAppDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase(configuration.GetSection("MongoDatabase:DatabaseName").Value);

        var lessonsCollectionName = configuration.GetSection("MongoDatabase:LessonsCollectionName").Value;

        Lessons = _database.GetCollection<Lesson>(lessonsCollectionName);
        Lessons.Indexes.CreateOne(
            new CreateIndexModel<Lesson>(Builders<Lesson>.IndexKeys.Ascending(m => m.EditedLessonId)));
    }

    /// <inheritdoc />
    public IMongoCollection<Lesson> Lessons { get; }

    /// <inheritdoc />
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}