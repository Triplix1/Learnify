using Learnify.Core.Domain.Entities.NoSql;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// CourseLessonContent repository
/// </summary>
public interface ICourseLessonContentRepository: IBaseMongoRepository<CourseLessonContent, string>
{
    
}