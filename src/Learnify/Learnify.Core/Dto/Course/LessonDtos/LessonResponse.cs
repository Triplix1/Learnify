using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.Subtitles;

namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonResponse
{
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
    /// Gets or sets value for VideoUrl
    /// </summary>
    public string? VideoUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for VideoBlobName
    /// </summary>
    public string? VideoBlobName { get; set; }
    
    /// <summary>
    /// Gets or sets value for VideoContainerName
    /// </summary>
    public string? VideoContainerName { get; set; }
    
    /// <summary>
    /// Gets or sets value for SubtitlesList
    /// </summary>
    public IList<SubtitlesResponse> SubtitlesList { get; set; }
    
    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IList<QuizQuestionResponse> Quizzes { get; set; }
    
    /// <summary>
    /// User has opportunity to create Attachements
    /// </summary>
    public IList<AttachmentResponse> Attachments { get; set; }
}