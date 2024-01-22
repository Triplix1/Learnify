namespace Notification.Dto;

public class MailDto
{
    public IEnumerable<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}