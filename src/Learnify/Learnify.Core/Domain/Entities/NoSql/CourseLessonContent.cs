﻿using MongoDB.Bson.Serialization.Attributes;

namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// CourseLessonContent
/// </summary>
public class CourseLessonContent
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    [BsonId]
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets value for Title
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Gets or sets value for Content
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// Gets or sets value for CreatedAt
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets value for UpdatedAt
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets value for Attachments
    /// </summary>
    public List<Attachment> Attachments { get; set; } 
    
    /// <summary>
    /// Gets or sets value for ImageUrl
    /// </summary>
    public string? VideoUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for ImageBlobName
    /// </summary>
    public string? VideoBlobName { get; set; }
    
    /// <summary>
    /// Gets or sets value for ImageContainerName
    /// </summary>
    public string? VideoContainerName { get; set; }

    // Interactivity Features
    
    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public List<QuizQuestion> Quizzes { get; set; }
}