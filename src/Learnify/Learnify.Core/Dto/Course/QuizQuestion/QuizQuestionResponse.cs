using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionResponse
{
    public string QuizId { get; set; }
    public AttachmentResponse? Media { get; set; }
    public AnswersResponse Answers { get; set; }
}