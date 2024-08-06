namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Mongo UnitOfWork
/// </summary>
public interface IMongoUnitOfWork
{
    /// <summary>
    /// Get value of CourseRepository
    /// </summary>
    ICourseRepository CourseRepository { get; }
    
    /// <summary>
    /// Get value of CourseLessonContentRepository
    /// </summary>
    ICourseLessonContentRepository CourseLessonContentRepository { get; }
    
    /// <summary>
    /// Get value of CourseLessonContentRepository
    /// </summary>
    IViewsRepository Views { get; }
}