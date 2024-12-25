using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// QuizQuestion
/// </summary>
public class QuizQuestion
{
    /// <summary>
    /// Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets value for Media
    /// </summary>
    public Attachment? Media { get; set; }
    
    /// <summary>
    /// Question
    /// </summary>
    public string Question { get; set; }
    
    /// <summary>
    /// Options
    /// </summary>
    public IEnumerable<string> Options { get; set; }
    
    /// <summary>
    /// CorrectAnswer
    /// </summary>
    public string CorrectAnswer { get; set; }
}