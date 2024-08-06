using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// CourseRating Repository
/// </summary>
public class CourseRatingPsqRepository: BasePsqRepository<CourseRating, int>, ICourseRatingsRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="CourseRatingPsqRepository"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public CourseRatingPsqRepository(ApplicationDbContext context) : base(context)
    {
    }
}