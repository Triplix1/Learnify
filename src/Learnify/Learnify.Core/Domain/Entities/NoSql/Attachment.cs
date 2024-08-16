using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// Attachment
/// </summary>
public class Attachment
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
    /// Gets or sets value for FileContainerName
    /// </summary>
    [CanBeNull]
    public string ConstantUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for ContentType
    /// </summary>
    public string ContentType { get; set; }
}