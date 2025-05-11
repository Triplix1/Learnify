using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Dto.Course.ParagraphDtos;

public class ParagraphUpdateRequest
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
}