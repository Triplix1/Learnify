namespace Learnify.Contracts;

public class SubtitlesGeneratedResponse
{
    public int SubtitleId { get; set; }
    public GeneratedResponseUpdateRequest SubtitleFileInfo { get; set; }
    public GeneratedResponseUpdateRequest TranscriptionFileInfo { get; set; }
}