using Learnify.Core.Dto.Subtitles;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ISubtitlesRepository
{
    Task<SubtitlesResponse> GetByIdAsync(int id);
    Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> id);
}