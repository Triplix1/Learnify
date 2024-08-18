using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

/// <inheritdoc />
public class LessonManager: ILessonManager
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    
    /// <summary>
    /// Initializes a new instance of <see cref="LessonManager"/>
    /// </summary>
    /// <param name="mongoUnitOfWork"><see cref="IMongoUnitOfWork"/></param>
    /// <param name="blobStorage"><see cref="IBlobStorage"/></param>
    public LessonManager(IMongoUnitOfWork mongoUnitOfWork, IBlobStorage blobStorage, IPsqUnitOfWork psqUnitOfWork)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _blobStorage = blobStorage;
        _psqUnitOfWork = psqUnitOfWork;
    }

    /// <inheritdoc />
    public async Task DeleteLessonsByParagraph(int paragraphId)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForParagraphAsync(paragraphId);

        var attachmentFileIds = attachments.Select(a => a.FileId);
        
        var fileDatas = await _psqUnitOfWork.PrivateFileRepository.GetByIdsAsync(attachmentFileIds);
        
        foreach (var fileData in fileDatas)
        {
            await _blobStorage.DeleteAsync(fileData.ContainerName, fileData.BlobName);
        }

        await _mongoUnitOfWork.Lessons.DeleteForParagraphAsync(paragraphId);
    }

    /// <inheritdoc />
    public async Task DeleteLessonsByParagraphs(IEnumerable<int> paragraphIds)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForParagraphsAsync(paragraphIds);

        await _mongoUnitOfWork.Lessons.DeleteForParagraphsAsync(paragraphIds);
    }
}