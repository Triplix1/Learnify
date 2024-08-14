using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class LessonService: ILessonService
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;

    public LessonService(IMongoUnitOfWork mongoUnitOfWork, IPsqUnitOfWork psqUnitOfWork)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _psqUnitOfWork = psqUnitOfWork;
    }

    public async Task<ApiResponse> DeleteAsync(string id)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonAsync(id);

        foreach (var attachment in attachments)
        {
            await _blobStorage.DeleteAsync(attachment.FileContainerName, attachment.FileBlobName);
        }

        await _mongoUnitOfWork.Lessons.DeleteAsync(id);

        return ApiResponse.Success();
    }

    public async Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonForUpdate(id);

        if (lesson is null)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(lesson.ParagraphId);

        if (userId != authorId)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));
        
        var 
    }

    public async Task<ApiResponse<LessonUpdateResponse>> CreateOrUpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<LessonResponse>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<AttachmentCreatedResponse> FillUrlsForBlob()
    {
        
    }
}