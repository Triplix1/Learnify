using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ILessonRepository
{
    Task<LessonUpdateResponse> GetLessonForUpdate(string id);
    Task<LessonResponse> GetLessonById(string id);
    Task<IEnumerable<LessonTitleResponse>> GetLessonsForParagraphAsync(int paragraphId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonAsync(string lessonId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphAsync(int paragraphId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphsAsync(IEnumerable<int> paragraphId);
    Task<int> GetParagraphIdForLesson(string lessonId);

    Task<LessonResponse> CreateAsync(Lesson lessonCreateRequest);
    Task<LessonResponse> UpdateAsync(Lesson lessonUpdateRequest);
    
    Task<bool> DeleteAsync(string id);
    Task<long> DeleteForParagraphAsync(int paragraphId);
    Task<long> DeleteForParagraphsAsync(IEnumerable<int> paragraphIds);
}