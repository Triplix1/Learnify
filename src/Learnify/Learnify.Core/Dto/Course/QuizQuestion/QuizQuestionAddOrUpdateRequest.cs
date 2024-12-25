using Learnify.Core.Dto.Attachment;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionAddOrUpdateRequest
{
    public string QuizId { get; set; }

    public string LessonId { get; set; }

    public AttachmentResponse? Media { get; set; }

    public string Question { get; set; }

    public IEnumerable<string> Options { get; set; }

    public int CorrectAnswer { get; set; }
}