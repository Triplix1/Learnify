using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;

namespace Learnify.Core.ManagerContracts;

public interface ISubtitlesManager
{
    Task<SubtitlesResponse> GetSubtitleByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);

    Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SubtitleReference>> CreateAsync(string fileBlobName, string containerName,
        IEnumerable<Language> subtitlesLanguages, Language primaryLanguage, int? courseId,
        CancellationToken cancellationToken = default);

    Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}