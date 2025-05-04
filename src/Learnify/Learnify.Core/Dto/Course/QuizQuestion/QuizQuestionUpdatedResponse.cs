using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionUpdatedResponse
{
    public string Id { get; set; }
    public string Question { get; set; }
    public AnswersUpdateResponse Answers { get; set; }
    public CurrentLessonUpdatedResponse CurrentLessonUpdated { get; set; }

}