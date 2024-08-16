using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
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
    private readonly IMapper _mapper;
    private readonly IRedisCacheManager _redisCacheManager;

    public LessonService(IMongoUnitOfWork mongoUnitOfWork, IPsqUnitOfWork psqUnitOfWork, IMapper mapper, IBlobStorage blobStorage, IRedisCacheManager redisCacheManager)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _blobStorage = blobStorage;
        _redisCacheManager = redisCacheManager;
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(string id, int userId)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonAsync(id);

        foreach (var attachment in attachments)
        {
            await _blobStorage.DeleteAsync(attachment.FileContainerName, attachment.FileBlobName);
        }

        await _mongoUnitOfWork.Lessons.DeleteAsync(id);

        return ApiResponse.Success();
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonForUpdate(id);

        if (lesson is null)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        var currParagraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLesson(id);
        
        var actualAuthorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(currParagraphId);

        if (actualAuthorId != userId)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));
        
        if(lesson.Video is not null)

        return ApiResponse<LessonUpdateResponse>.Success(lesson);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonResponse>> CreateAsync(LessonCreateRequest lessonCreateRequest, int userId)
    {
        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(lessonCreateRequest.ParagraphId);

        if (userId != authorId)
            return ApiResponse<LessonResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));

        var lesson = _mapper.Map<Lesson>(lessonCreateRequest);
        
        //TODO: Here should be logic for generating subtitles

        var createdLesson = await _mongoUnitOfWork.Lessons.CreateAsync(lesson);

        return ApiResponse<LessonResponse>.Success(createdLesson);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonResponse>> UpdateAsync(LessonUpdateRequest lessonUpdateRequest, int userId)
    {
        var currParagraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLesson(lessonUpdateRequest.Id);
        
        var actualAuthorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(currParagraphId);
        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(lessonUpdateRequest.ParagraphId);

        if (actualAuthorId != authorId || authorId != userId)
            return ApiResponse<LessonResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));

        var oldAttachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonAsync(lessonUpdateRequest.Id);

        var currAttachments = new List<AttachmentCreatedResponse>(lessonUpdateRequest.Attachments);

        if (lessonUpdateRequest.Video is not null)
        {
            currAttachments.Add(lessonUpdateRequest.Video);
            if (oldAttachments.All(oat => oat.FileBlobName != lessonUpdateRequest.Video.FileBlobName))
            {
                
            }
        }
        else
        {
            //TODO: Delete
        }
        
        currAttachments.AddRange(lessonUpdateRequest.Quizzes.Select(q => q.Media));

        
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonResponse>> GetByIdAsync(string id, int userId)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonById(id);
        
        if (lesson is null)
            return ApiResponse<LessonResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        return ApiResponse<LessonResponse>.Success(lesson);
    }
}