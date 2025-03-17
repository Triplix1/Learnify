namespace Learnify.Core.Dto.MeetingConnection;

public class MeetingConnectionCreateRequest
{
    public string ConnectionId { get; set; }
    public string SessionId { get; set; }
    public int UserId { get; set; }
    public bool IsAuthor { get; set; }
}