using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ILessonRepository
{
    Task<Lesson> GetLessonByIdAsync(string id);
    Task<IEnumerable<LessonTitleResponse>> GetLessonTitlesForParagraphAsync(int paragraphId, bool includeDrafts);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonAsync(string lessonId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphAsync(int paragraphId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphsAsync(IEnumerable<int> paragraphId);
    Task<int> GetParagraphIdForLessonAsync(string lessonId);

    Task<Lesson> CreateAsync(Lesson lessonCreateRequest);
    Task<Lesson> UpdateAsync(Lesson lessonUpdateRequest);
    
    Task<bool> DeleteAsync(string id);
    Task<long> DeleteForParagraphAsync(int paragraphId);
    Task<long> DeleteForParagraphsAsync(IEnumerable<int> paragraphIds);
}