using Learnify.Core.Dto.Course.QuizQuestion.Answers;

namespace Learnify.Core.ServiceContracts;

public interface IAnswersService
{
    Task<AnswersUpdatedResponse> AddOrUpdateAnswersAsync(int userId, AnswerAddOrUpdateRequest answerAddOrUpdateRequest, CancellationToken cancellationToken = default);
}