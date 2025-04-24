using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Domain.Entities.Sql;

public class MeetingStream
{
    [Key]
    public string StreamId { get; set; }

    public string ConnectionId { get; set; }
    public MeetingConnection Connection { get; set; }
}