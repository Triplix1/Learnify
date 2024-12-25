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
    Task<PagedList<Paragraph>> GetFilteredAsync(EfFilter<Paragraph> filter, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Returns entity by id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Paragraph?> GetByIdAsync(int key, IEnumerable<string> stringsToInclude = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Paragraph> CreateAsync(Paragraph entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Paragraph> UpdateAsync(Paragraph entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<int?> GetAuthorIdAsync(int id, CancellationToken cancellationToken = default);
}