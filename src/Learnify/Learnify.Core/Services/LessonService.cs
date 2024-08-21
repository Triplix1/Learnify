using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.Subtitles;
using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;
using MassTransit;

namespace Learnify.Core.Services;

public class LessonService: ILessonService
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ISubtitlesManager _subtitlesManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public LessonService(IMongoUnitOfWork mongoUnitOfWork, IPsqUnitOfWork psqUnitOfWork, IMapper mapper,
        ISubtitlesManager subtitlesManager, IPublishEndpoint publishEndpoint)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _subtitlesManager = subtitlesManager;
        _publishEndpoint = publishEndpoint;
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(string id, int userId)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonAsync(id);
        
        var attachmentFileIds = attachments.Select(a => a.FileId);
        
        await _psqUnitOfWork.PrivateFileRepository.DeleteRangeAsync(attachmentFileIds);

        await _mongoUnitOfWork.Lessons.DeleteAsync(id);

        return ApiResponse.Success();
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonForUpdateAsync(id);

        if (lesson is null)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        var currParagraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(id);
        
        var actualAuthorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(currParagraphId);

        if (actualAuthorId != userId)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));
        
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

        lesson.Video.Subtitles = await AddNewVideoAsync(lessonCreateRequest.Video);
        
        var createdLesson = await _mongoUnitOfWork.Lessons.CreateAsync(lesson);

        return ApiResponse<LessonResponse>.Success(createdLesson);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonResponse>> UpdateAsync(LessonUpdateRequest lessonUpdateRequest, int userId)
    {
        var currParagraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(lessonUpdateRequest.Id);

        var actualAuthorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(currParagraphId);
        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorId(lessonUpdateRequest.ParagraphId);

        if (actualAuthorId != authorId || authorId != userId)
            return ApiResponse<LessonResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));

        var oldLesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonUpdateRequest.Id);
        var updatedLesson = _mapper.Map<Lesson>(lessonUpdateRequest);


        if (lessonUpdateRequest.Video?.Attachment.FileId != oldLesson.Video?.Attachment.FileId)
        {
            if (oldLesson.Video is not null)
            {
                var subtitleIds = oldLesson.Video.Subtitles.Select(s => s.Id);
                await _subtitlesManager.DeleteRangeAsync(subtitleIds);
            }

            if (lessonUpdateRequest.Video?.Attachment?.FileId != null)
            {
                updatedLesson.Video.Subtitles = await AddNewVideoAsync(lessonUpdateRequest.Video);
            }
        }
        else if (lessonUpdateRequest.Video is not null && oldLesson.Video is not null)
        {
            var subtitlesDiff =
                oldLesson.Video.Subtitles.Where(s => !lessonUpdateRequest.Video.Subtitles.Contains(s.Language));

            var subtitlesDiffIds = subtitlesDiff.Select(s => s.Id);

            var diffSubtitleReferences =
                _mapper.Map<IEnumerable<SubtitleReference>>(oldLesson.Video.Subtitles.Except(subtitlesDiff));

            var updatedLessonSubtitles = new List<SubtitleReference>(diffSubtitleReferences);

            await _subtitlesManager.DeleteRangeAsync(subtitlesDiffIds);

            var subtitlesToCreate = lessonUpdateRequest.Video.Subtitles.Where(l =>
                !oldLesson.Video.Subtitles.Select(s => s.Language).Contains(l)).Select(l => new SubtitlesCreateRequest()
            {
                Language = l
            });

            var createdSubtitles = await _psqUnitOfWork.SubtitlesRepository.CreateRangeAsync(subtitlesToCreate);

            var newSubtitleReferences = _mapper.Map<IEnumerable<SubtitleReference>>(createdSubtitles);
            updatedLessonSubtitles.AddRange(newSubtitleReferences);
            updatedLesson.Video.Subtitles = updatedLessonSubtitles;

            var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(lessonUpdateRequest.Video.Attachment
                .FileId);

            var subtitlesInfo = createdSubtitles.Select(s => new SubtitleInfo()
            {
                Id = s.Id,
                Language = s.Language.ToString()
            });

            var generateRequest = new SubtitlesGenerateRequest()
            {
                VideoBlobName = file.BlobName,
                VideoContainerName = file.ContainerName,
                SubtitleInfo = subtitlesInfo,
                PrimaryLanguage = lessonUpdateRequest.Video.PrimaryLanguage.ToString()
            };

            await _publishEndpoint.Publish(generateRequest);
        }

        var response = await _mongoUnitOfWork.Lessons.UpdateAsync(updatedLesson);

        return ApiResponse<LessonResponse>.Success(response);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonResponse>> GetByIdAsync(string id, int userId)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(id);
        
        if (lesson is null)
            return ApiResponse<LessonResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        return ApiResponse<LessonResponse>.Success(lesson);
    }

    private async Task<IEnumerable<SubtitleReference>> AddNewVideoAsync(VideoAddOrUpdateRequest videoAddOrUpdateRequest)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(videoAddOrUpdateRequest.Attachment
            .FileId);
        var subtitlesCreateRequest = videoAddOrUpdateRequest.Subtitles.Select(s => new SubtitlesCreateRequest
        {
            Language = s
        });
        var subtitles = await _psqUnitOfWork.SubtitlesRepository.CreateRangeAsync(subtitlesCreateRequest);

        var subtitlesInfo = _mapper.Map<IEnumerable<SubtitleInfo>>(subtitles);

        var subtitlesGenerateRequest = new SubtitlesGenerateRequest
        {
            VideoBlobName = file.BlobName,
            VideoContainerName = file.ContainerName,
            SubtitleInfo = subtitlesInfo,
            PrimaryLanguage = videoAddOrUpdateRequest.PrimaryLanguage.ToString()
        };

        await _publishEndpoint.Publish(subtitlesGenerateRequest);
        
        var result = _mapper.Map<IEnumerable<SubtitleReference>>(subtitles);

        return result;
    }
}