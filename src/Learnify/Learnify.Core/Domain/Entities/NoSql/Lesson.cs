namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// Course lesson
/// </summary>
public class Lesson
{
    /// <summary>
    /// Gets or sets value for CourseId
    /// </summary>
    public int CourseId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Course
    /// </summary>
    public Course Course { get; set; }
    
    /// <summary>
    /// Id reference to content of lesson, which stores in NoSql
    /// </summary>
    public string ContentId { get; set; }
}