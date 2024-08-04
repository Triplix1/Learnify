namespace Learnify.Core.Domain.Entities.NoSql;

public class Paragraph
{
    public string Name { get; set; }
    public IList<Lesson> Lessons { get; set; }
}