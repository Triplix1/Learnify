using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface ILessonRepository
{
    Task<IEnumerable<LessonTitleResponse>> GetLessonsForParagraphAsync(int paragraphId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonAsync(string lessonId);
    Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonsAsync(IEnumerable<string> lessonIds);
    Task<bool> DeleteAsync(string id);
    Task<long> DeleteForParagraphAsync(int paragraphId);
    Task<long> DeleteForParagraphsAsync(IEnumerable<int> paragraphIds);
}