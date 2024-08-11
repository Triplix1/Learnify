using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

public class CourseUpdateRequest
{
    /// <summary>
    /// Gets or sets value for Id
    /// </summary>
    public int Id { get; set; }
    
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
}