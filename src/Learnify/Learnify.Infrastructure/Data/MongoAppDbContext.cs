using Learnify.Core.Domain.Entities;
using Learnify.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Data;

/// <inheritdoc />
public class MongoAppDbContext: IMongoAppDbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="MongoAppDbContext"/>
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    public MongoAppDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetSection("MongoDatabase:ConnectionString").Value);
        var database = client.GetDatabase(configuration.GetSection("MongoDatabase:DatabaseName").Value);

        var viewsCollectionName = configuration.GetSection("MongoDatabase:ViewsCollectionName").Value;
        
        var filter = new BsonDocument("name", viewsCollectionName);
        var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });
        if (!collections.Any())
        {
            var options = new CreateCollectionOptions { TimeSeriesOptions = new TimeSeriesOptions("time") };
            database.CreateCollection(viewsCollectionName, options);
        }
        else
        {
            Views = database.GetCollection<View>(viewsCollectionName);   
        }
    }
    
    /// <inheritdoc />
    public IMongoCollection<View> Views { get; }
}