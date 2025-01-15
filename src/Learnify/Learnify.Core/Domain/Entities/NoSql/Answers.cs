namespace Learnify.Core.Domain.Entities.NoSql;

public class Answers
{
    public IEnumerable<string> Options { get; set; }
    public int CorrectAnswer { get; set; }
}