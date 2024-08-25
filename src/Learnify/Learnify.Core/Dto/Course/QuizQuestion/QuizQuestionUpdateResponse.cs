using Learnify.Core.Dto.Attachment;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionUpdateResponse
{
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
    public IEnumerable<string> Options { get; set; }
    
    /// <summary>
    /// CorrectAnswer
    /// </summary>
    public int CorrectAnswer { get; set; }
}