using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion;

namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonCreateRequest
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
    /// Gets or sets value for Video
    /// </summary>
    public AttachmentResponse Video { get; set; }

    /// <summary>
    /// User has opportunity to create quizzes
    /// </summary>
    public IEnumerable<QuizQuestionAddOrUpdateRequest> Quizzes { get; set; }

}