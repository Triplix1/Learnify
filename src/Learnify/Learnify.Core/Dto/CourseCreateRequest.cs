using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Learnify.Core.Dto;

/// <summary>
/// CourseCreateRequest
/// </summary>
public class CourseCreateRequest
{
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