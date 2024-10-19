using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.ServiceContracts;

public interface ILessonService
{
    Task<ApiResponse> DeleteAsync(string id, int userId, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<LessonTitleResponse>>> GetByParagraphAsync(int paragraphId, int userId,
        bool includeDrafts = false, CancellationToken cancellationToken = default);

    Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<LessonUpdateResponse>> AddOrUpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, CancellationToken cancellationToken = default);

    Task<ApiResponse<LessonUpdateResponse>> SaveDraftAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, CancellationToken cancellationToken = default);

    Task<ApiResponse<LessonResponse>>
        GetByIdAsync(string id, int userId, CancellationToken cancellationToken = default);
}