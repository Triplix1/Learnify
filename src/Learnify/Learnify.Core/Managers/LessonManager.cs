using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Managers;

/// <inheritdoc />
public class LessonManager : ILessonManager
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="LessonManager"/>
    /// </summary>
    /// <param name="mongoUnitOfWork"><see cref="IMongoUnitOfWork"/></param>
    /// <param name="blobStorage"><see cref="IBlobStorage"/></param>
    public LessonManager(IMongoUnitOfWork mongoUnitOfWork, IBlobStorage blobStorage, IPsqUnitOfWork psqUnitOfWork,
        IMapper mapper)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _blobStorage = blobStorage;
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
    }

    public async Task<string> GetLessonToUpdateId(string lessonId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(lessonId))
            return null;

        var currentLesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonId);

        if (currentLesson == null)
            return null;
        
        var lessonAddOrUpdateRequest = _mapper.Map<LessonAddOrUpdateRequest>(currentLesson);
        
        if (lessonAddOrUpdateRequest.Id is null)
            return await CreateAsync(lessonAddOrUpdateRequest, true, cancellationToken);

        if (lessonAddOrUpdateRequest.EditedLessonId is not null)
        {
            lessonAddOrUpdateRequest.Id = lessonAddOrUpdateRequest.EditedLessonId;
            lessonAddOrUpdateRequest.EditedLessonId = null;

            return await UpdateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);
        }

        var originalLesson =
            await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonAddOrUpdateRequest.Id, cancellationToken);

        if (originalLesson.IsDraft) 
            return await UpdateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);

        lessonAddOrUpdateRequest.Id = null;

        var draft = await CreateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);
    }

    public async Task<LessonUpdateResponse> CreateAsync(LessonAddOrUpdateRequest lessonCreateRequest,
        bool draft = false, CancellationToken cancellationToken = default)
    {
        var lesson = _mapper.Map<Lesson>(lessonCreateRequest);
        lesson.IsDraft = draft;

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            if (lessonCreateRequest.Video is not null)
                lesson.Video.Subtitles = await AddNewVideoAsync(lessonCreateRequest.Video, cancellationToken);

            var createdLesson = await _mongoUnitOfWork.Lessons.CreateAsync(lesson, cancellationToken);

            ts.Complete();

            var response = await GetUpdateResponseAsync(createdLesson, cancellationToken);

            return LessonUpdateResponse.Success(response);
        }
    }

    /// <inheritdoc />
    public async Task DeleteLessonsByParagraph(int paragraphId, CancellationToken cancellationToken = default)
    {
        var attachments =
            await _mongoUnitOfWork.Lessons.GetAllAttachmentsForParagraphAsync(paragraphId, cancellationToken);

        var attachmentFileIds = attachments.Select(a => a.FileId);

        var fileDatas = await _psqUnitOfWork.PrivateFileRepository.GetByIdsAsync(attachmentFileIds, cancellationToken);

        foreach (var fileData in fileDatas)
            await _blobStorage.DeleteAsync(fileData.ContainerName, fileData.BlobName, cancellationToken);

        await _mongoUnitOfWork.Lessons.DeleteForParagraphAsync(paragraphId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteLessonsByParagraphs(IEnumerable<int> paragraphIds,
        CancellationToken cancellationToken = default)
    {
        var attachments =
            await _mongoUnitOfWork.Lessons.GetAllAttachmentsForParagraphsAsync(paragraphIds, cancellationToken);

        await _mongoUnitOfWork.Lessons.DeleteForParagraphsAsync(paragraphIds, cancellationToken);
    }
}