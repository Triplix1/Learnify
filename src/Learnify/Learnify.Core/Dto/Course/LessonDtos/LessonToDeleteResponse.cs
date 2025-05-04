namespace Learnify.Core.Dto.Course.LessonDtos;

public class LessonToDeleteResponse
{
    public string Id { get; set; }
    public string EditedLessonId { get; set; }
    public string OriginalLessonId { get; set; }
    public bool IsDraft { get; set; }
    public IEnumerable<int> Attachments { get; set; }
    public IEnumerable<int> Subtitles { get; set; }
    public IEnumerable<string> Quizzes { get; set; }
}