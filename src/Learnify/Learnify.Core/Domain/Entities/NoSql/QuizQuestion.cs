namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// QuizQuestion
/// </summary>
public class QuizQuestion
{
    /// <summary>
    /// Gets or sets value for Media
    /// </summary>
    public Attachment? Media { get; set; }
    
    /// <summary>
    /// Options
    /// </summary>
    public IEnumerable<string> Options { get; set; }
    
    /// <summary>
    /// CorrectAnswer
    /// </summary>
    public string CorrectAnswer { get; set; }
}