using Learnify.Core.Domain.Entities.NoSql;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Courses repository
/// </summary>
public interface ICourseRepository : IBaseRepository<Course, string>
{
    
}