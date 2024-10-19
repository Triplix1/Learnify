using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto.Subtitles;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ISubtitlesRepository
{
    Task<Subtitle> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subtitle>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<Subtitle> CreateAsync(Subtitle subtitlesCreateRequest, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subtitle>> CreateRangeAsync(IEnumerable<Subtitle> subtitlesCreateRequest, CancellationToken cancellationToken = default);
    Task<Subtitle> UpdateAsync(Subtitle subtitlesUpdateRequest, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteRangeAsync(IEnumerable<int> id, CancellationToken cancellationToken = default);
}