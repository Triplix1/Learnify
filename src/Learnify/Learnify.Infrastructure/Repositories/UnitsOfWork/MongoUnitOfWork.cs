using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;

/// <inheritdoc />
public class MongoUnitOfWork: IMongoUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of <see cref="MongoUnitOfWork"/>
    /// </summary>
    /// <param name="view"><see cref="IViewRepository"/></param>
    /// <param name="lessons"><see cref="ILessonRepository"/></param>
    public MongoUnitOfWork(IViewRepository view, ILessonRepository lessons)
    {
        View = view;
        Lessons = lessons;
    }
    
    /// <inheritdoc />
    public IViewRepository View { get; }

    /// <inheritdoc />
    public ILessonRepository Lessons { get; }
}