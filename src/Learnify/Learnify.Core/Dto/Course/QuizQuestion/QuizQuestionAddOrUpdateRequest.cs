namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionAddOrUpdateRequest
{
    public string QuizId { get; set; }
    public string LessonId { get; set; }
    public string Question { get; set; }
}