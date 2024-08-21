namespace Learnify.Contracts;

public class SubtitlesGenerateRequest
{
    public string VideoContainerName { get; set; }
    public string VideoBlobName { get; set; }
    public string PrimaryLanguage { get; set; }
    public IEnumerable<SubtitleInfo> SubtitleInfo { get; set; }
}