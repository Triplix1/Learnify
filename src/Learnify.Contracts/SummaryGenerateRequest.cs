namespace Learnify.Contracts;

public class SummaryGenerateRequest
{
    public string VideoContainerName { get; set; }
    public string VideoBlobName { get; set; }
    public string ContentType { get; set; }
    public int FileId { get; set; }
    public string Language { get; set; }
}