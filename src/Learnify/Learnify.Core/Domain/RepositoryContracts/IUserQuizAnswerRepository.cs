﻿using Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IUserQuizAnswerRepository
{
    Task<IEnumerable<UserQuizAnswerResponse>> GetUserQuizAnswersForLessonAsync(int userId, string lessonId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<UserQuizAnswerResponse>> SaveUserAnswersAsync(int userId, string lessonId, IEnumerable<UserQuizAnswerCreateRequest> userQuizAnswerCreateRequests,
        CancellationToken cancellationToken = default);
}