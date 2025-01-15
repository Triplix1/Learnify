using Learnify.Core.Domain.Entities.NoSql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IAnswerRepository
{
    Task<int> GetCorrectAnswerAsync(string lessonId, string quizId, CancellationToken cancellationToken = default);
    Task<Answers> AddOrUpdateAnswerAsync(string lessonId, string quizId, Answers answers, CancellationToken cancellationToken = default);
}