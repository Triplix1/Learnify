namespace Learnify.Core.Domain.Entities;

/// <summary>
/// Course rating entity
/// </summary>
public class CourseRating: BaseEntity
{
    /// <summary>
    /// Gets or sets value for UserId
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets value for CourseId
    /// </summary>
    public int CourseId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Rate
    /// </summary>
    public byte Rate { get; set; }
}