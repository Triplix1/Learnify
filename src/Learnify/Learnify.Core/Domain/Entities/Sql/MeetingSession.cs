using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Domain.Entities.Sql;

public class MeetingSession
{
    [Key]
    public string SessionId { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
}