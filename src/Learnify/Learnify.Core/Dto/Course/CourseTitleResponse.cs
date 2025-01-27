using Learnify.Core.Dto.File;

namespace Learnify.Core.Dto.Course;

public class CourseTitleResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PrivateFileDataResponse Photo { get; set; }
}