using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.ParagraphDtos;

namespace Learnify.Core.ServiceContracts;

public interface ILessonService
{
    Task<LessonResponse> GetByIdAsync(string id, int userId, CancellationToken cancellationToken = default);
    
    Task<string> GetLessonToUpdateIdAsync(string lessonId, int userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<LessonTitleResponse>> GetByParagraphAsync(int paragraphId, int userId,
        bool includeDrafts = false, CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> GetForUpdateAsync(string id, int userId,
        CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> GetUpdateResponseAsync(Lesson lesson,
        CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> AddOrUpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, CancellationToken cancellationToken = default);

    Task<LessonUpdateResponse> SaveDraftAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, CancellationToken cancellationToken = default);

    Task<LessonDeletedResponse> DeleteAsync(string id, int userId, CancellationToken cancellationToken = default);
    Task DeleteByParagraphAsync(int paragraphId, int userId, CancellationToken cancellationToken = default);
}