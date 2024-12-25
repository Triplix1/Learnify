using Learnify.Core.Domain.Entities.NoSql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IQuizRepository
{
    Task<IEnumerable<QuizQuestion>> GetQuizzesByLessonIdAsync(string lessonId,
        CancellationToken cancellationToken = default);

    Task<QuizQuestion> UpdateAsync(QuizQuestion quiz, string lessonId,
        CancellationToken cancellationToken = default);

    Task<QuizQuestion> CreateAsync(QuizQuestion quiz, string lessonId,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string id, string lessonId, CancellationToken cancellationToken = default);
}