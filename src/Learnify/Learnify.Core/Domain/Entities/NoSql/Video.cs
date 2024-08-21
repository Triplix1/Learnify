using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.NoSql;

public class Video
{
    public Attachment Attachment { get; set; }
    public Language PrimaryLanguage { get; set; }
    public IEnumerable<SubtitleReference> Subtitles { get; set; }
}