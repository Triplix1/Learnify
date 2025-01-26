using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

public class CourseUpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Language PrimaryLanguage { get; set; }
}