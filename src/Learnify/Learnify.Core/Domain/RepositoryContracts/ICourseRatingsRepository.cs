using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.Base;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Interface for CourseRatingsRepository
/// </summary>
public interface ICourseRatingsRepository: IBasePsqRepository<CourseRating, int>
{
    
}