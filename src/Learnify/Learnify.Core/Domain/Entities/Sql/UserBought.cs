namespace Learnify.Core.Domain.Entities.Sql;

public class UserBought
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public User User { get; set; }
    public Course Course { get; set; }
}