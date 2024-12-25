using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.QuizQuestion;

namespace Learnify.Core.ServiceContracts;

public interface IQuizService
{
    Task<QuizQuestionUpdateResponse> AddOrUpdateQuizAsync(QuizQuestionAddOrUpdateRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteQuizAsync(string id, string lessonId,
        CancellationToken cancellationToken = default);
}