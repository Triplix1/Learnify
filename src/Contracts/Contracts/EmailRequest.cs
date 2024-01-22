namespace Contracts;

public class EmailRequest
{
    public IEnumerable<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}