namespace Learnify.Core.Dto.Course.QuizQuestion.Answers;

public class AnswersValidateRequest
{
    public string LessonId { get; set; }
    public IList<QuizValidateRequest> QuizValidateRequests { get; set; }
}