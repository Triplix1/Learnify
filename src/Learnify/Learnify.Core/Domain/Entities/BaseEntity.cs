namespace Learnify.Core.Domain.Entities;

/// <summary>
/// Base generic class for entities
/// </summary>
public class BaseEntity<TKey>: BaseEntity
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public TKey Id { get; set; }
}

/// <summary>
/// Base class for entities
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Gets or sets value for CreatedAt
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets value for UpdatedAt
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}