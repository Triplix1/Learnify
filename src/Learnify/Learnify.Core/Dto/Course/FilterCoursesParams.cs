using Learnify.Core.Dto.Params;

namespace Learnify.Core.Dto.Course;

public record FilterCoursesParams
{
    public CourseParams CourseParams { get; set; }
    public int? AuthorId { get; set; }
    public int? UserId { get; set; }
    public bool PublishedOnly { get; set; } = true;
}