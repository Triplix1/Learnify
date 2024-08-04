using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Courses repo
/// </summary>
public class CourseRepository: BaseMongoRepository<Course, string>, ICourseRepository
{
    /// <summary>
    /// Courses constructor
    /// </summary>
    /// <param name="context"><see cref="IMongoAppDbContext"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    public CourseRepository(IMongoAppDbContext context, IConfiguration configuration) : base(context,
        configuration["MongoDatabase:CoursesCollectionName"])
    {
    }
}