using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// CourseRating Repository
/// </summary>
public class CourseRatingRepository: BaseRepository<CourseRating, int>, ICourseRatingsRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="CourseRatingRepository"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public CourseRatingRepository(ApplicationDbContext context) : base(context)
    {
    }
}