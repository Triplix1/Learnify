using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Dto.Course.ParagraphDtos;

public class ParagraphCreateRequest
{
    public int CourseId { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
}