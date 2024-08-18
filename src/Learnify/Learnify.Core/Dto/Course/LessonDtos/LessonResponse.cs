using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.Subtitles;

namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonResponse
{
    /// <summary>
    /// Id
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
    /// Gets or sets value for SubtitlesList
    /// </summary>
    public IEnumerable<SubtitlesReferenceResponse> SubtitlesList { get; set; }
    
    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IEnumerable<QuizQuestionResponse> Quizzes { get; set; }
}