using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Domain.Entities.Sql;

public class Group
{
    [Key]
    public string Name { get; set; }
    public virtual ICollection<Connection> Connections { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}
