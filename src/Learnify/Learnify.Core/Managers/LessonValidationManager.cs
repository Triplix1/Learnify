using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto.ExceptionResponses;
using Learnify.Core.Exceptions;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class LessonValidationManager: ILessonValidationManager
{
    public void ValidatePossibilityToSaveLesson(Lesson lesson)
    {
        var compositeErrors = new List<string>();

        if (lesson.Quizzes.Any(l => l.Answers.Options.Count() < 2))
            compositeErrors.Add("You should add at least 2 options to each quiz");
        
        if(lesson.Video == null)
            compositeErrors.Add("Lesson should contain video");

        if (compositeErrors.Any())
        {
            var error = new LessonSavingErrors(compositeErrors);
            
            throw new CompositeException(error);
        }
    }
}