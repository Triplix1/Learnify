using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion;

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
    /// Gets or sets value for Content
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// Gets or sets value for Video
    /// </summary>
    public AttachmentResponse Video { get; set; }
    
    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IEnumerable<QuizQuestionAddOrUpdateRequest> Quizzes { get; set; }
    
    /// <summary>
    /// User has opportunity to create Attachments
    /// </summary>
    public IEnumerable<AttachmentResponse> Attachments { get; set; }
}