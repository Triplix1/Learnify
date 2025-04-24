namespace Learnify.Core.Domain.Entities.Sql;

public class Message
{
    public int Id { get; set; }
    public int? SenderId { get; set; }
    public string GroupId { get; set; }
    public string Content { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    
    public User Sender { get; set; }
    public Group Group { get; set; }
}
