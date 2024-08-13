using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;

/// <inheritdoc />
public class MongoUnitOfWork: IMongoUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of <see cref="MongoUnitOfWork"/>
    /// </summary>
    /// <param name="views"><see cref="IViewsRepository"/></param>
    /// <param name="lessons"><see cref="ILessonRepository"/></param>
    public MongoUnitOfWork(IViewsRepository views, ILessonRepository lessons)
    {
        Views = views;
        Lessons = lessons;
    }
    
    /// <inheritdoc />
    public IViewsRepository Views { get; }

    /// <inheritdoc />
    public ILessonRepository Lessons { get; }
}