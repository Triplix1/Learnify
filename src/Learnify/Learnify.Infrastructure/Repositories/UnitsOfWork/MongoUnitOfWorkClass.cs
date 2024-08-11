using Learnify.Core.Domain.RepositoryContracts;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class MongoUnitOfWorkClass: IMongoUnitOfWork
{
    public MongoUnitOfWorkClass(ICourseRepository courseRepository,
        ICourseLessonContentRepository courseLessonContentRepository, IViewsRepository views)
    {
        CourseRepository = courseRepository;
        CourseLessonContentRepository = courseLessonContentRepository;
        Views = views;
    }

    /// <inheritdoc />
    public ICourseRepository CourseRepository { get; }

    /// <inheritdoc />
    public ICourseLessonContentRepository CourseLessonContentRepository { get; }

    /// <inheritdoc />
    public IViewsRepository Views { get; }
}