using Learnify.Core.Dto.Course.QuizQuestion;

namespace Learnify.Core.ServiceContracts;

public interface IQuizService
{
    Task<QuizQuestionUpdateResponse> AddOrUpdateQuizAsync(QuizQuestionAddOrUpdateRequest request, int userId,
        CancellationToken cancellationToken = default);

    Task DeleteQuizAsync(string id, string lessonId, int userId,
        CancellationToken cancellationToken = default);
}