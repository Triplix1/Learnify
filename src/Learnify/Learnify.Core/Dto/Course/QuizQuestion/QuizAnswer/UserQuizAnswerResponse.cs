namespace Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;

public class UserQuizAnswerResponse
{
    public string QuizId { get; set; }
    public int AnswerIndex { get; set; }
    public bool IsCorrect { get; set; }
}