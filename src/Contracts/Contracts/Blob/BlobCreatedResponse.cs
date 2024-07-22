namespace Contracts.Blob;

/// <summary>
/// Response entity, result of creating blob
/// </summary>
public class BlobCreatedResponse
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
    /// Gets or sets value for Url
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Gets or sets value for ContentType
    /// </summary>
    public string ContentType { get; set; }
}