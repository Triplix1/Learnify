using Learnify.Core.Dto.Subtitles;

namespace Learnify.Core.ManagerContracts;

public interface ISubtitlesManager
{
    Task<SubtitlesResponse> GetSubtitleByIdAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
    Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest);
    Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest);
    Task<bool> DeleteAsync(int id);
}