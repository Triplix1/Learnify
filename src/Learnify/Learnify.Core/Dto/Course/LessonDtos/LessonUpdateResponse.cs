using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.LessonDtos;

/// <summary>
/// LessonUpdateResponse
/// </summary>
public class LessonUpdateResponse
{
    public string Id { get; set; }
    public int ParagraphId { get; set; }
    public string Title { get; set; }
    public string EditedLessonId { get; set; }
    public string OriginalLessonId { get; set; }
    public bool IsDraft { get; set; }
    public Language PrimaryLanguage { get; set; }

    public VideoResponse Video { get; set; }
    
    public IEnumerable<QuizQuestionUpdateResponse> Quizzes { get; set; }
}