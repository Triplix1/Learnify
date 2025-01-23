using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.Video;

namespace Learnify.Core.Dto.Course.LessonDtos;

/// <summary>
/// LessonUpdateResponse
/// </summary>
public class LessonUpdateResponse
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
    /// Gets or sets value for Title
    /// </summary>
    public string OriginalLessonId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Title
    /// </summary>
    public bool IsDraft { get; set; }
    
    /// <summary>
    /// Gets or sets value for Video
    /// </summary>
    public VideoResponse Video { get; set; }
    
    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IEnumerable<QuizQuestionUpdateResponse> Quizzes { get; set; }
}