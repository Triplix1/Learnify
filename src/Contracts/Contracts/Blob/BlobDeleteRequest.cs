namespace Contracts.Blob;

/// <summary>
/// Request entity for deleting blob
/// </summary>
public class BlobDeleteRequest
{
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for ContainerName
    /// </summary>
    public string ContainerName { get; set; }
}