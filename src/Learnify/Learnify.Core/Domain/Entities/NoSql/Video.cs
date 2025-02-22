namespace Learnify.Core.Domain.Entities.NoSql;

public class Video
{
    public Attachment Attachment { get; set; }
    public IEnumerable<SubtitleReference> Subtitles { get; set; }
}