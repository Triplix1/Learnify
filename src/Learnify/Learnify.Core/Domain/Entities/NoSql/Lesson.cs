namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// Paragraph's lesson
/// </summary>
public class Lesson
{
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Id reference to content of lesson, which stores in NoSql
    /// </summary>
    public string ContentId { get; set; }
}