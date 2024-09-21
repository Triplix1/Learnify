namespace Learnify.Core.Dto.Messages;

public class MessageResponse
{
    public int Id { get; set; }
    public string SenderName { get; set; }
    public int? SenderId { get; set; }
    public string Content { get; set; }
    public DateTime MessageSent { get; set; }
}