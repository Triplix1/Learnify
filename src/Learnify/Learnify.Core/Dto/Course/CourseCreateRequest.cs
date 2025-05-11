using System.ComponentModel.DataAnnotations;
using Learnify.Core.Dto.Course.Interfaces;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course;

public class CourseCreateRequest: ICourseUpdatable
{
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    [Range(1, int.MaxValue)]
    public decimal Price { get; set; }


    public Language PrimaryLanguage { get; set; }
}