using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto.Subtitles;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ISubtitlesRepository
{
    Task<Subtitle> GetByIdAsync(int id);
    Task<IEnumerable<Subtitle>> GetByIdsAsync(IEnumerable<int> ids);
    Task<Subtitle> CreateAsync(Subtitle subtitlesCreateRequest);
    Task<IEnumerable<Subtitle>> CreateRangeAsync(IEnumerable<Subtitle> subtitlesCreateRequest);
    Task<Subtitle> UpdateAsync(Subtitle subtitlesUpdateRequest);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> id);
}