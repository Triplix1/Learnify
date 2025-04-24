namespace Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

public interface IMongoUnitOfWork
{
    ILessonRepository Lessons { get; }
    IQuizRepository Quizzes { get; }
    IAnswerRepository Answers { get; }
}