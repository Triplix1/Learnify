using Learnify.Core.Domain.RepositoryContracts;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class MongoUnitOfWorkClass: IMongoUnitOfWork
{
    public MongoUnitOfWorkClass(ICourseRepository courseRepository, ICourseLessonContentRepository courseLessonContentRepository)
    {
        CourseRepository = courseRepository;
        CourseLessonContentRepository = courseLessonContentRepository;
    }

    /// <inheritdoc />
    public ICourseRepository CourseRepository { get; }

    /// <inheritdoc />
    public ICourseLessonContentRepository CourseLessonContentRepository { get; }
}