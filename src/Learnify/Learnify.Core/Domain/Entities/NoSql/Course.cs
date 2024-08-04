using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.NoSql;

/// <summary>
/// Course entity
/// </summary>
public class Course: BaseEntity<string>
{
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
    /// Gets or sets value for PrimaryLanguage
    /// </summary>
    public Language PrimaryLanguage { get; set; }
    
    /// <summary>
    /// Gets or sets value for Lessons
    /// </summary>
    public IList<Paragraph> Paragraphs { get; set; }
}