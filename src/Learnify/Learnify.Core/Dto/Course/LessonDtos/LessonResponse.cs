using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;
using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonResponse
{
    public string Id { get; set; }
    
    public int ParagraphId { get; set; }
    
    public string Title { get; set; }
    public Language PrimaryLanguage { get; set; }
    public VideoResponse Video { get; set; }
    public IEnumerable<QuizQuestionResponse> Quizzes { get; set; }
}