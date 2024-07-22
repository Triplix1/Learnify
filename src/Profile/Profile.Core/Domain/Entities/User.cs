using System.ComponentModel.DataAnnotations;

namespace Profile.Core.Domain.Entities;

/// <summary>
/// Profile entity
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    [Required]
    [Key]
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets value for UserName
    /// </summary>
    [Required]
    public string UserName { get; set; }
    
    /// <summary>
    /// Gets or sets value for Email
    /// </summary>
    [Required]
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets value for Type
    /// </summary>
    [Required]
    public UserType Type { get; set; }
    
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for Surname
    /// </summary>
    [Required]
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
    /// Gets or sets value for ImageUrl
    /// </summary>
    public string? ImageUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for ImageBlobName
    /// </summary>
    public string? ImageBlobName { get; set; }
    
    /// <summary>
    /// Gets or sets value for ImageContainerName
    /// </summary>
    public string? ImageContainerName { get; set; }
}