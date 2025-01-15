using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;

public class MongoUnitOfWork: IMongoUnitOfWork
{
    public MongoUnitOfWork(IViewRepository view, ILessonRepository lessons)
    {
        View = view;
        Lessons = lessons;
    }
    
    public IViewRepository View { get; }

    public ILessonRepository Lessons { get; }
    
    public IQuizRepository Quizzes { get; }
    
    public IAnswerRepository Answers { get; }
}