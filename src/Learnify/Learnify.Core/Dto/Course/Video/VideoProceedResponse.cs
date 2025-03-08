using Learnify.Core.Domain.Entities.NoSql;

namespace Learnify.Core.Dto.Course.Video;

public class VideoProceedResponse
{
    public IEnumerable<SubtitleReference> Subtitles { get; set; }
    public int SummaryFileId { get; set; }
}