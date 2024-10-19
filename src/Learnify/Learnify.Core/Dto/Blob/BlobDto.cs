namespace Learnify.Core.Dto.Blob;

/// <summary>
/// BlobDto
/// </summary>
public class BlobDto
{
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for ContainerName
    /// </summary>
    public string ContainerName { get; set; }
    
    /// <summary>
    /// Gets or sets value for ContentType
    /// </summary>
    public string ContentType { get; set; }
    
    /// <summary>
    /// Gets or sets value for Content
    /// </summary>
    public Stream Content { get; set; }
}