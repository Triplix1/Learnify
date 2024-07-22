namespace Contracts.User;

/// <summary>
/// Message that user have been updated
/// </summary>
public class UserUpdated
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets value for Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for Surname
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Gets or sets value for Username
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Gets or sets value for ImageUrl
    /// </summary>
    public string ImageUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for Role
    /// </summary>
    public string Role { get; set; }
}