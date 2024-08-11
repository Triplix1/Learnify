using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Lessons repository
/// </summary>
public class CourseLessonContentRepository: ICourseLessonContentRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="CourseLessonContentRepository"/>
    /// </summary>
    /// <param name="context"><see cref="IMongoAppDbContext"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    protected CourseLessonContentRepository(IMongoAppDbContext context, IConfiguration configuration) : base(context,
        configuration["MongoDatabase:CourseLessonsCollectionName"])
    {
    }
}