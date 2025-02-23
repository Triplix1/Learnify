namespace Learnify.Contracts;

public class SubtitlesGeneratedResponse
{
    public int SubtitleId { get; set; }
    public string LessonId { get; set; }
    public GeneratedResponseUpdateRequest SubtitleFileInfo { get; set; }
}