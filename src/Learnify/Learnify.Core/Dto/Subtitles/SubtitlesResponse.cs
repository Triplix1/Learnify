using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Subtitles;

public class SubtitlesResponse
{ 
    public int Id { get; set; }
    public int? SubtitleFileId { get; set; }
    public int? TranscriptionFileId { get; set; }
    public Language Language { get; set; }
}