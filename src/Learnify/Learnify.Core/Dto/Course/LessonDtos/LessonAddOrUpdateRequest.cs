using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonAddOrUpdateRequest
{
    public string Id { get; set; }
    public int ParagraphId { get; set; }
    public string Title { get; set; }
    public string EditedLessonId { get; set; }
    public Language PrimaryLanguage { get; set; }
    public VideoAddOrUpdateRequest Video { get; set; }
}