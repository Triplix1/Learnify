using Learnify.Core.Dto.Attachment;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionResponse
{
    /// <summary>
    /// Id
    /// </summary>

    public string Id { get; set; }
    /// <summary>
    /// Gets or sets value for Media
    /// </summary>
    public AttachmentResponse? Media { get; set; }
    
    /// <summary>
    /// Question
    /// </summary>
    public string Question { get; set; }
    
    /// <summary>
    /// Options
    /// </summary>
    public List<string> Options { get; set; }
}