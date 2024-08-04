namespace Learnify.Core.Domain.Entities.NoSql;

public class QuizQuestion
{
    public string Question { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAnswer { get; set; }
}