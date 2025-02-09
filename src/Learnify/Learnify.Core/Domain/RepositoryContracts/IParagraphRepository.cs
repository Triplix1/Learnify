using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IParagraphRepository
{
    Task<PagedList<Paragraph>> GetFilteredAsync(EfFilter<Paragraph> filter, CancellationToken cancellationToken = default);
    
    Task<Paragraph> GetByIdAsync(int key, IEnumerable<string> stringsToInclude = null, CancellationToken cancellationToken = default);
    
    Task<int?> GetAuthorIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<int?> GetCourseIdAsync(int paragraphId, CancellationToken cancellationToken = default);
    
    Task<Paragraph> CreateAsync(Paragraph entity, CancellationToken cancellationToken = default);
    
    Task<Paragraph> UpdateAsync(Paragraph entity, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}