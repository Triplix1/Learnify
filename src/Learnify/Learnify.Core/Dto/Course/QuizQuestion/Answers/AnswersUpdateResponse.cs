namespace Learnify.Core.Dto.Course.QuizQuestion.Answers;

public class AnswersUpdateResponse
{
    public IEnumerable<string> Options { get; set; }
    public int CorrectAnswer { get; set; }
}