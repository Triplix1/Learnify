using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Subtitles;

public class SubtitlesUpdateRequest
{
    public int Id { get; set; }
    public int? FileId { get; set; } 
    public Language Language { get; set; }
}