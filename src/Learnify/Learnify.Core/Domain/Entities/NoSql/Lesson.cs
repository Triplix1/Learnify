using JetBrains.Annotations;
using Learnify.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// CourseLessonContent
/// </summary>
public class Lesson: BaseEntity
{
    /// <summary>
    /// Id
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets value for ParagraphId
    /// </summary>
    public int ParagraphId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Title
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Gets or sets value for Content
    /// </summary>
    [CanBeNull]
    public string Content { get; set; }
    
    /// <summary>
    /// Gets or sets value for Video
    /// </summary>
    [CanBeNull]
    public Attachment Video { get; set; }
    
    /// <summary>
    /// Gets or sets value for SubtitlesList
    /// </summary>
    public IList<Subtitles> Subtitles { get; set; }
    
    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IList<QuizQuestion> Quizzes { get; set; }
    
    /// <summary>
    /// User has opportunity to create Attachements
    /// </summary>
    public IList<Attachment> Attachments { get; set; }

    public LessonStates State { get; set; }
}