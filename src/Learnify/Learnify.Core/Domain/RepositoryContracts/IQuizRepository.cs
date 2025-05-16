using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Course.QuizQuestion;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IQuizRepository
{
    Task<IEnumerable<QuizQuestion>> GetQuizzesByLessonIdAsync(string lessonId,
        CancellationToken cancellationToken = default);

    Task<QuizQuestion> UpdateAsync(QuizQuestionAddOrUpdateRequest quiz,
        CancellationToken cancellationToken = default);

    Task<QuizQuestion> CreateAsync(QuizQuestionAddOrUpdateRequest quiz,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string id, string lessonId, CancellationToken cancellationToken = default);
}