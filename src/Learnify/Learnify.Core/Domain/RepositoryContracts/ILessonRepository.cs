using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ILessonRepository
{
    Task<Lesson> GetLessonByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<string> GetLessonToUpdateIdForCurrentLessonAsync(string lessonId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<LessonTitleResponse>> GetLessonTitlesForParagraphAsync(int paragraphId, bool includeDrafts, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonAsync(string lessonId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphAsync(int paragraphId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphsAsync(IEnumerable<int> paragraphId, CancellationToken cancellationToken = default);
    Task<int> GetParagraphIdForLessonAsync(string lessonId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubtitleReference>> GetSubtitleReferencesForLessonAsync(string lessonId,
        CancellationToken cancellationToken = default);
    Task<LessonToDeleteResponse> GetLessonToDelete(string lessonId, CancellationToken cancellationToken = default);

    Task<Lesson> CreateAsync(Lesson lessonCreateRequest, CancellationToken cancellationToken = default);
    Task<Lesson> UpdateAsync(Lesson lessonUpdateRequest, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<long> DeleteForParagraphAsync(int paragraphId, CancellationToken cancellationToken = default);
    Task<long> DeleteForParagraphsAsync(IEnumerable<int> paragraphIds, CancellationToken cancellationToken = default);
}