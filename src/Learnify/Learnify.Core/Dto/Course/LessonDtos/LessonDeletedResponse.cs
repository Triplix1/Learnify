using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonDeletedResponse
{
    public ParagraphPublishedResponse ParagraphPublishedResponse { get; set; }
    public bool UnpublishedParagraph { get; set; }
}