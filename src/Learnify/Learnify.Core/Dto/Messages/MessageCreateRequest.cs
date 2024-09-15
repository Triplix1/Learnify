namespace Learnify.Core.Dto.Messages;

public class MessageCreateRequest
{
    public int SenderId { get; set; }
    public string GroupName { get; set; }
    public string Content { get; set; }
}