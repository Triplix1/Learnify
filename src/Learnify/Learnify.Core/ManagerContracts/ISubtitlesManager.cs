using Learnify.Core.Dto.Subtitles;

namespace Learnify.Core.ManagerContracts;

public interface ISubtitlesManager
{
    Task<SubtitlesResponse> GetSubtitleByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);

    Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest,
        CancellationToken cancellationToken = default);

    Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}