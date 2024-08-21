using Learnify.Core.Dto.Subtitles;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ISubtitlesRepository
{
    Task<SubtitlesResponse> GetByIdAsync(int id);
    Task<IEnumerable<SubtitlesResponse>> GetByIdsAsync(IEnumerable<int> ids);
    Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest);
    Task<IEnumerable<SubtitlesResponse>> CreateRangeAsync(IEnumerable<SubtitlesCreateRequest> subtitlesCreateRequest);
    Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> id);
}