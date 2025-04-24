namespace Learnify.Core.Domain.Entities.Sql;

public class UserQuizAnswer
{
    public int UserId { get; set; }
    public string QuizId { get; set; }
    public string LessonId { get; set; }
    public int AnswerIndex { get; set; }
}