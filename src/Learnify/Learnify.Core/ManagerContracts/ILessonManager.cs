namespace Learnify.Core.ManagerContracts;

public interface ILessonManager
{
    Task DeleteLessonsByParagraph(int paragraphId);
    Task DeleteLessonsByParagraphs(IEnumerable<int> paragraphIds);
}