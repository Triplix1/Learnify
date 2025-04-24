namespace Learnify.Core.Domain.Entities;

/// <summary>
/// Base generic class for entities
/// </summary>
public class BaseEntity<TKey>
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public TKey Id { get; set; }
}