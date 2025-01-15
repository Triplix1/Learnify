using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionUpdateResponse
{
    public string QuizId { get; set; }
    public AttachmentResponse? Media { get; set; }
    public string Question { get; set; }
    public AnswersUpdateResponse Answers { get; set; }
}