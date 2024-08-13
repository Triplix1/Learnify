namespace Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

/// <summary>
/// Mongo UnitOfWork
/// </summary>
public interface IMongoUnitOfWork
{
    /// <summary>
    /// Get value of Views
    /// </summary>
    IViewsRepository Views { get; }
    /// <summary>
    /// Get value of LLessonRepository
    /// </summary>
    ILessonRepository Lessons { get; }
}