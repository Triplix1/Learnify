using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Dto.Course.QuizQuestion.Answers;

public class AnswersUpdatedResponse
{
    public IEnumerable<string> Options { get; set; }
    public int CorrectAnswer { get; set; }
    public CurrentLessonUpdatedResponse CurrentLessonUpdated { get; set; }
}