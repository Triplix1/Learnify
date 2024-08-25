using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.Video;

namespace Learnify.Core.Dto.Course.LessonDtos;

/// <summary>
/// LessonAddOrUpdateRequest
/// </summary>
public class LessonAddOrUpdateRequest
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets value for ParagraphId
    /// </summary>
    public int ParagraphId { get; set; }

    /// <summary>
    /// Gets or sets value for Title
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Gets or sets value for Title
    /// </summary>
    public string EditedLessonId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Video
    /// </summary>
    public VideoAddOrUpdateRequest Video { get; set; }

    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IEnumerable<QuizQuestionAddOrUpdateRequest> Quizzes { get; set; }
}