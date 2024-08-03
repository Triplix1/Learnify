namespace Learnify.Core.Dto;

/// <summary>
/// MailDto
/// </summary>
public class MailDto
{
    /// <summary>
    /// Gets or sets value for To
    /// </summary>
    public IEnumerable<string> To { get; set; }
    
    /// <summary>
    /// Gets or sets value for Subject
    /// </summary>
    public string Subject { get; set; }
    
    /// <summary>
    /// Gets or sets value for Content
    /// </summary>
    public string Content { get; set; }
}