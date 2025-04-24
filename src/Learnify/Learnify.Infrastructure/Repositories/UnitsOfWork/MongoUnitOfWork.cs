using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;

public class MongoUnitOfWork: IMongoUnitOfWork
{
    public MongoUnitOfWork(ILessonRepository lessons, IQuizRepository quizzes, IAnswerRepository answers)
    {
        Lessons = lessons;
        Quizzes = quizzes;
        Answers = answers;
    }

    public ILessonRepository Lessons { get; }
    
    public IQuizRepository Quizzes { get; }
    
    public IAnswerRepository Answers { get; }
}