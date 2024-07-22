using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Profile.Core.DTO;

/// <summary>
/// ProfileUpdateRequest
/// </summary>
public class ProfileUpdateRequest
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    [Required]
    public string Id { get; set; }
    
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
    /// Gets or sets value for File
    /// </summary>
    public IFormFile? File { get; set; }
}