namespace Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

public interface IMongoUnitOfWork
{
    IViewRepository View { get; }
    ILessonRepository Lessons { get; }
    IQuizRepository Quizzes { get; }
    IAnswerRepository Answers { get; }
}