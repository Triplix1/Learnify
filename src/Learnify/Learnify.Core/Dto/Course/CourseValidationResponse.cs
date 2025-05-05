using Learnify.Core.Dto.Course.Interfaces;
using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.Dto.Course;

public class CourseValidationResponse: ICourseUpdatable
{
    public int Id { get; set; }
    public int? PhotoId { get; set; }
    public int? VideoId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public ICollection<ParagraphResponse> Paragraphs { get; set; }

}