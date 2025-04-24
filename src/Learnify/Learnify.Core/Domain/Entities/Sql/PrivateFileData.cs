namespace Learnify.Core.Domain.Entities.Sql;

public class PrivateFileData: BaseEntity<int>
{
    public string ContentType { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }
    public int? CourseId { get; set; }
}