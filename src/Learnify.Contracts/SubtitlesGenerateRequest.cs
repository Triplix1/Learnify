namespace Learnify.Contracts;

public class SubtitlesGenerateRequest
{
    public string LessonId { get; set; }
    public string VideoContainerName { get; set; }
    public string VideoBlobName { get; set; }
    public SubtitleInfo SubtitleInfo { get; set; }
}