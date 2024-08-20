using AutoMapper;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class FileService: IFileService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IBlobStorage _blobStorage;

    public FileService(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMongoUnitOfWork mongoUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task<Stream> GetFileStreamById(int id, int userId)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id);

        if (file is null)
            throw new KeyNotFoundException("Cannot find file with such id");

        if (file.LessonId is not null)
        {
            var paragraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(file.LessonId);

            var paragraph = await _psqUnitOfWork.ParagraphRepository.GetByIdAsync(paragraphId);

            if (paragraph is null)
                throw new KeyNotFoundException("Cannot find file with such id");
            
            var userHasBoughtThisCourse = await _psqUnitOfWork.UserBoughtRepository.UserBoughtExists(userId, paragraph.CourseId);

            if (!userHasBoughtThisCourse)
                throw new Exception("User has not access for this file");
        }

        var stream = await _blobStorage.GetBlobStreamAsync(file.ContainerName, file.BlobName);

        return stream;
    }
}