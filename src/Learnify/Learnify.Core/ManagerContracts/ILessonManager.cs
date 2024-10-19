namespace Learnify.Core.ManagerContracts;

public interface ILessonManager
{
    Task DeleteLessonsByParagraph(int paragraphId, CancellationToken cancellationToken = default);
    Task DeleteLessonsByParagraphs(IEnumerable<int> paragraphIds, CancellationToken cancellationToken = default);
}