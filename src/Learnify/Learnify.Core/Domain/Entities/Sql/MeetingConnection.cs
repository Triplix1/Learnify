using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Domain.Entities.Sql;

public class MeetingConnection
{
    [Key]
    public string ConnectionId { get; set; }

    public string SessionId { get; set; }
    public int UserId { get; set; }
    public bool IsAuthor { get; set; }

    public MeetingSession Session { get; set; }
    public User User { get; set; }
}