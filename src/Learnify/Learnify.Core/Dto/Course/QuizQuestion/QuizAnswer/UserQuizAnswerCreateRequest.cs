namespace Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;

public class UserQuizAnswerCreateRequest
{
    public string QuizId { get; set; }
    public int AnswerIndex { get; set; }
}