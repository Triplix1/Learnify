using Learnify.Core.Dto.Course.QuizQuestion.Answers;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionUpdateResponse
{
    public string Id { get; set; }
    public string Question { get; set; }
    public AnswersUpdateResponse Answers { get; set; }
}