namespace Learnify.Core.Dto.ExceptionResponses;

public class LessonSavingErrors
{
    public List<string> CompositeErrors { get; set; }

    public LessonSavingErrors(List<string> compositeErrors)
    {
        CompositeErrors = compositeErrors;
    }
}