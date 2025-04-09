using Learnify.Core.Domain.Entities.NoSql;

namespace Learnify.Core.ManagerContracts;

public interface ILessonValidationManager
{
    void ValidatePossibilityToSaveLesson(Lesson lesson);
}