using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.Subtitles;

public class SubtitlesReferenceUpdateRequest
{
    public int SubtitleId { get; set; }
    public Language Language;
}