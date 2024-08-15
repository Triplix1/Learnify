namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonAttachmentsResponse
{
    public Domain.Entities.NoSql.Attachment Video { get; set; }
    public IEnumerable<Domain.Entities.NoSql.Attachment> Subtitles { get; set; }
    public IEnumerable<Domain.Entities.NoSql.Attachment> Quizzes { get; set; }
    public IEnumerable<Domain.Entities.NoSql.Attachment> Attachments { get; set; }
}