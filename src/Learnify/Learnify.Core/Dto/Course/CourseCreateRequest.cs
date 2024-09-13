using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

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
    
    // /// <summary>
    // /// Gets or sets value for PrimaryLanguage
    // /// </summary>
    public Language PrimaryLanguage { get; set; }
}