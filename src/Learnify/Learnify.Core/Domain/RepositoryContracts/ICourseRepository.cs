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
    Task<PagedList<Course>> GetFilteredAsync(EfFilter<Course> filter);
    
    /// <summary>
    /// Returns entity by id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Course> GetByIdAsync(int key, IEnumerable<string> includes);

    Task<Course> PublishAsync(int key, bool publish);
    
    /// <summary>
    /// Creates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Course> CreateAsync(Course entity);
    
    /// <summary>
    /// Updates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Course> UpdateAsync(Course entity);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<bool> DeleteAsync(int id);
    
    /// <summary>
    /// Returns courseId
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns>Success of operation</returns>
    Task<int?> GetAuthorId(int courseId);
}