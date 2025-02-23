using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;

namespace Learnify.Core.ManagerContracts;

public interface ISubtitlesManager
{
    Task<SubtitlesResponse> GetSubtitleByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<SubtitlesResponse> CreateAsync(
        int? courseId,
        SubtitlesCreateRequest subtitlesCreateRequest,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<SubtitlesResponse>> CreateSubtitlesAsync(
        int? courseId,
        IEnumerable<SubtitlesCreateRequest> subtitlesCreateRequests,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<SubtitleReference>> CreateAndGenerateAsync(
        SubtitlesCreateAndGenerateRequest subtitlesCreateAndGenerateRequest,
        CancellationToken cancellationToken = default);

    Task RequestSubtitlesTranslationAsync(int baseSubtitleId, IEnumerable<int> targetSubtitlesId, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}