namespace Learnify.Contracts;

public class TranslateRequest
{
    public string MainFileContainerName { get; set; }
    public string MainFileBlobName { get; set; }
    public string MainLanguage { get; set; }
    public IEnumerable<TranslateFileDataRequest> TranslateRequests { get; set; }
}