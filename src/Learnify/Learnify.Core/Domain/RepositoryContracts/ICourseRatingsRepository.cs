using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Interface for CourseRatingsRepository
/// </summary>
public interface ICourseRatingsRepository: IBasePsqRepository<CourseRating, int>
{
    
}