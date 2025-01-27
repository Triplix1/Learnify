using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

public class CourseCreateRequest
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public Language PrimaryLanguage { get; set; }
}