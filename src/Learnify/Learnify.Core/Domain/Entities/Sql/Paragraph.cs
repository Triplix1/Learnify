namespace Learnify.Core.Domain.Entities.Sql;

public class Paragraph: BaseEntity<int>
{
    public int CourseId { get; set; }
    public bool IsPublished { get; set; }
    public string Name { get; set; }
    public Course Course { get; set; }
}