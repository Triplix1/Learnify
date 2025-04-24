using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Domain.Entities.Sql;

public class Connection
{
    [Key]
    public string ConnectionId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string GroupName { get; set; }
    public Group Group { get; set; }
}
