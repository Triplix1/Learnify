namespace Learnify.Core.Domain.Entities;

/// <summary>
/// View entity
/// </summary>
public class View
{
    /// <summary>
    /// Gets or sets value for CourseId
    /// </summary>
    public int CourseId { get; set; }
    
    /// <summary>
    /// Gets or sets value for UserId
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Time
    /// </summary>
    public DateTime Time { get; set; }
}