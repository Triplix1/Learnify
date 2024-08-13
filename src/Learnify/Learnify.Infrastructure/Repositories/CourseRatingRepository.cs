using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Learnify.Infrastructure.Repositories.Base;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// CourseRating Repository
/// </summary>
public class CourseRatingRepository: BasePsqRepository<CourseRating, int>, ICourseRatingsRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="CourseRatingRepository"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public CourseRatingRepository(ApplicationDbContext context) : base(context)
    {
    }
}