using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

public class CourseResponse
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets value for AuthorId
    /// </summary>
    public int AuthorId { get; set; }
    
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for Price
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets value for IsPublished
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Gets or sets value for PrimaryLanguage
    /// </summary>
    public string PrimaryLanguage { get; set; }
    
    /// <summary>
    /// Gets or sets value for Lessons
    /// </summary>
    public IEnumerable<ParagraphResponse> Paragraphs { get; set; }
}