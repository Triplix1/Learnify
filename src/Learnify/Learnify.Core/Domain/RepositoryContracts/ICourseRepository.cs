using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Courses repository
/// </summary>
public interface ICourseRepository
{
    /// <summary>
    /// Gets filtered entities
    /// </summary>
    /// <param name="filter"><see cref="MongoFilter{T}"/></param>
    /// <returns></returns>
    Task<PagedList<Course>> GetFilteredAsync(EfFilter<Course> filter, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Returns entity by id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Course> GetByIdAsync(int key, IEnumerable<string> includes = null, CancellationToken cancellationToken = default);

    Task<Course> PublishAsync(int key, bool publish, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Course> CreateAsync(Course entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Course> UpdateAsync(Course entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Returns courseId
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns>Success of operation</returns>
    Task<int?> GetAuthorIdAsync(int courseId, CancellationToken cancellationToken = default);
}