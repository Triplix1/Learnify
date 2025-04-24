using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.Sql;

public class Subtitle: BaseEntity<int>
{
    public int? SubtitleFileId { get; set; }
    public Language Language { get; set; }
    public PrivateFileData SubtitleFile { get; set; }
}