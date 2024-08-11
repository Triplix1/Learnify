using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class LessonManager: ILessonManager
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public LessonManager(IMongoUnitOfWork mongoUnitOfWork)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task<bool> DeleteLessonsByParagraph(int paragraphId)
    {
        
        _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonsAsync();
    }
}