using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;

namespace Learnify.Core.ServiceContracts;

public interface IQuizService
{
    Task<QuizQuestionUpdatedResponse> AddOrUpdateQuizAsync(QuizQuestionAddOrUpdateRequest request, int userId,
        CancellationToken cancellationToken = default);

    Task<CurrentLessonUpdatedResponse> DeleteQuizAsync(string id, string lessonId, int userId,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<UserQuizAnswerResponse>> CheckAnswersAsync(AnswersValidateRequest request, int userId, CancellationToken cancellationToken = default);
}