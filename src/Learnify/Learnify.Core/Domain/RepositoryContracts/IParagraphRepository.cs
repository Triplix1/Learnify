using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// ParagraphRepository
/// </summary>
public interface IParagraphRepository
{
    /// <summary>
    /// Gets filtered entities
    /// </summary>
    /// <param name="filter"><see cref="EfFilter{Paragraph}"/></param>
    /// <returns></returns>
    Task<PagedList<Paragraph>> GetFilteredAsync(EfFilter<Paragraph> filter);
    
    /// <summary>
    /// Returns entity by id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Paragraph?> GetByIdAsync(int key);
    
    /// <summary>
    /// Creates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Paragraph> CreateAsync(Paragraph entity);
    
    /// <summary>
    /// Updates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Paragraph> UpdateAsync(Paragraph entity);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<bool> DeleteAsync(int id);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<int?> GetAuthorIdAsync(int id);
}