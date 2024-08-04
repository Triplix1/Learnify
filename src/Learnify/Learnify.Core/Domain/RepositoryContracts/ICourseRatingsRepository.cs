using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Interface for CourseRatingsRepository
/// </summary>
public interface ICourseRatingsRepository: IBaseRepository<CourseRating, int>
{
    
}