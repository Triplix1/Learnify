namespace Learnify.Core.Dto.Course.QuizQuestion.Answers;

public class AnswerAddOrUpdateRequest
{
    public string LessonId { get; set; }
    public string QuizId { get; set; }
    public IEnumerable<string> Options { get; set; }
    public int CorrectAnswer { get; set; }
}