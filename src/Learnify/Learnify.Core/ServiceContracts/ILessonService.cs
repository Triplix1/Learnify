using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.ServiceContracts;

public interface ILessonService
{
    Task DeleteAsync(string id, int userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<LessonTitleResponse>> GetByParagraphAsync(int paragraphId, int userId,
        bool includeDrafts = false, CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> GetForUpdateAsync(string id, int userId,
        CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> AddOrUpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> SaveDraftAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, CancellationToken cancellationToken = default);

    Task<LessonResponse> GetByIdAsync(string id, int userId, CancellationToken cancellationToken = default);
}