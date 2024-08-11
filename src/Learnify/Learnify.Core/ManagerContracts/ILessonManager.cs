namespace Learnify.Core.ManagerContracts;

public interface ILessonManager
{
    Task<bool> DeleteLessonsByParagraph(int paragraphId);
}