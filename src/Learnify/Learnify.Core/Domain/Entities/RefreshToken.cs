namespace Learnify.Core.Domain.Entities;

/// <summary>
/// Refresh token entity
/// </summary>
public class RefreshToken: BaseEntity
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public string Jwt { get; set; }
    
    /// <summary>
    /// Gets or sets value for Refresh
    /// </summary>
    public string Refresh { get; set; }
    
    /// <summary>
    /// Gets or sets value for Expire
    /// </summary>
    public DateTime Expire { get; set; }
    
    /// <summary>
    /// Gets or sets value for HasBeenUsed
    /// </summary>
    public bool HasBeenUsed { get; set; }
}