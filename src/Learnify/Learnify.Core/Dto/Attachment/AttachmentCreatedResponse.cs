namespace Learnify.Core.Dto.Attachment;

public class AttachmentCreatedResponse
{
    /// <summary>
    /// Gets or sets value for FileBlobName
    /// </summary>
    public string FileBlobName { get; set; }
    
    /// <summary>
    /// Gets or sets value for FileContainerName
    /// </summary>
    public string FileContainerName { get; set; }
    
    /// <summary>
    /// Gets or sets value for FileUrl
    /// </summary>
    public string FileUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for ContentType
    /// </summary>
    public string ContentType { get; set; }
}