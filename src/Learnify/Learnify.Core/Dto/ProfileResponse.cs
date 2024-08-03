using Learnify.Core.Enums;

namespace Learnify.Core.Dto;

/// <summary>
/// Profile response
/// </summary>
public class ProfileResponse
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets value for UserName
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Gets or sets value for Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets value for Type
    /// </summary>
    public Role Type { get; set; }
    
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for Surname
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Gets or sets value for Company
    /// </summary>
    public string? Company { get; set; }
    
    /// <summary>
    /// Gets or sets value for CardNumber
    /// </summary>
    public string? CardNumber { get; set; }
    
    /// <summary>
    /// Gets or sets value for PhotoUrl
    /// </summary>
    public string? PhotoUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for PhotoPublicId
    /// </summary>
    public string? PhotoPublicId { get; set; }
}