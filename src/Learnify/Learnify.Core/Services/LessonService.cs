﻿using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;
using MassTransit;

namespace Learnify.Core.Services;

public class LessonService : ILessonService
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ISubtitlesManager _subtitlesManager;
    private readonly IPrivateFileManager _privateFileManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public LessonService(IMongoUnitOfWork mongoUnitOfWork, IPsqUnitOfWork psqUnitOfWork, IMapper mapper,
        ISubtitlesManager subtitlesManager, IPublishEndpoint publishEndpoint, IPrivateFileManager privateFileManager)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _subtitlesManager = subtitlesManager;
        _publishEndpoint = publishEndpoint;
        _privateFileManager = privateFileManager;
    }

    /// <inheritdoc />
    public async Task<ApiResponse> DeleteAsync(string id, int userId, CancellationToken cancellationToken = default)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonAsync(id, cancellationToken);
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(id, cancellationToken);

        var attachmentFileIds = attachments.Select(a => a.FileId);

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            await _psqUnitOfWork.PrivateFileRepository.DeleteRangeAsync(attachmentFileIds, cancellationToken);

            if (lesson.Video != null)
            {
                var subtitleIds = lesson.Video.Subtitles.Select(s => s.SubtitleId);
                await _subtitlesManager.DeleteRangeAsync(subtitleIds, cancellationToken);
            }

            await _mongoUnitOfWork.Lessons.DeleteAsync(id, cancellationToken);

            ts.Complete();
        }
        
        return ApiResponse.Success();
    }

    public async Task<ApiResponse<IEnumerable<LessonTitleResponse>>> GetByParagraphAsync(int paragraphId, int userId,
        bool includeDrafts = false, CancellationToken cancellationToken = default)
    {
        if (includeDrafts)
        {
            var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(paragraphId, cancellationToken);
            if (userId != authorId)
                return ApiResponse<IEnumerable<LessonTitleResponse>>.Failure(
                    new Exception("You should be author of the course to be able see draft lessons"));
        }

        var response =
            await _mongoUnitOfWork.Lessons.GetLessonTitlesForParagraphAsync(paragraphId, includeDrafts,
                cancellationToken);

        return ApiResponse<IEnumerable<LessonTitleResponse>>.Success(response);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId,
        CancellationToken cancellationToken = default)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(id, cancellationToken);

        if (lesson is null)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        var currParagraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(id, cancellationToken);

        var actualAuthorId =
            await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(currParagraphId, cancellationToken);

        if (actualAuthorId != userId)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));

        var response = await GetUpdateResponseAsync(lesson, cancellationToken);

        return ApiResponse<LessonUpdateResponse>.Success(response);
    }

    public async Task<ApiResponse<LessonUpdateResponse>> AddOrUpdateAsync(
        LessonAddOrUpdateRequest lessonAddOrUpdateRequest, int userId, CancellationToken cancellationToken = default)
    {
        if (lessonAddOrUpdateRequest.Id is null)
            return await CreateAsync(lessonAddOrUpdateRequest, userId, cancellationToken: cancellationToken);

        return await UpdateAsync(lessonAddOrUpdateRequest, userId, cancellationToken: cancellationToken);
    }

    private async Task<ApiResponse<LessonUpdateResponse>> CreateAsync(LessonAddOrUpdateRequest lessonCreateRequest,
        int userId, bool draft = false, CancellationToken cancellationToken = default)
    {
        var authorId =
            await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(lessonCreateRequest.ParagraphId,
                cancellationToken);

        if (userId != authorId)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));

        var lesson = _mapper.Map<Lesson>(lessonCreateRequest);
        lesson.IsDraft = draft;

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            if (lessonCreateRequest.Video is not null)
                lesson.Video.Subtitles = await AddNewVideoAsync(lessonCreateRequest.Video, cancellationToken);

            var createdLesson = await _mongoUnitOfWork.Lessons.CreateAsync(lesson, cancellationToken);
            
            ts.Complete();
            
            var response = await GetUpdateResponseAsync(createdLesson, cancellationToken);

            return ApiResponse<LessonUpdateResponse>.Success(response);
        }
    }

    private async Task<ApiResponse<LessonUpdateResponse>> UpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, bool draft = false, CancellationToken cancellationToken = default)
    {
        var currParagraphId =
            await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(lessonAddOrUpdateRequest.Id, cancellationToken);

        var actualAuthorId =
            await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(currParagraphId, cancellationToken);
        var authorId =
            await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(lessonAddOrUpdateRequest.ParagraphId,
                cancellationToken);

        if (actualAuthorId != authorId || authorId != userId)
            return ApiResponse<LessonUpdateResponse>.Failure(
                new Exception("You should be author of the course to be able to edit it"));

        var oldLesson =
            await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonAddOrUpdateRequest.Id, cancellationToken);
        var updatedLesson = _mapper.Map<Lesson>(lessonAddOrUpdateRequest);
        updatedLesson.IsDraft = draft;

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            if (!draft && oldLesson.EditedLessonId is not null)
            {
                await _mongoUnitOfWork.Lessons.DeleteAsync(oldLesson.EditedLessonId, cancellationToken);
                updatedLesson.EditedLessonId = null;
            }

            var quizzesDiff =
                oldLesson.Quizzes.Where(q => q.Media != null && !updatedLesson.Quizzes.All(up => up.Media?.FileId != q.Media.FileId)).Select(q => q.Media.FileId);

            await _privateFileManager.DeleteRangeAsync(quizzesDiff, cancellationToken);
            
            if (lessonAddOrUpdateRequest.Video?.Attachment.FileId != oldLesson.Video?.Attachment.FileId)
            {
                if (oldLesson.Video is not null)
                {
                    var subtitleIds = oldLesson.Video.Subtitles.Select(s => s.SubtitleId);
                    await _privateFileManager.DeleteAsync(oldLesson.Video.Attachment.FileId, cancellationToken);
                    await _subtitlesManager.DeleteRangeAsync(subtitleIds, cancellationToken);
                }

                if (lessonAddOrUpdateRequest.Video?.Attachment?.FileId != null)
                    updatedLesson.Video.Subtitles =
                        await AddNewVideoAsync(lessonAddOrUpdateRequest.Video, cancellationToken);
            }
            else if (lessonAddOrUpdateRequest.Video is not null && oldLesson.Video is not null)
            {
                var subtitlesDiff =
                    oldLesson.Video.Subtitles.Where(s =>
                        !lessonAddOrUpdateRequest.Video.Subtitles.Contains(s.Language));

                var subtitlesDiffIds = subtitlesDiff.Select(s => s.SubtitleId);

                var diffSubtitleReferences =
                    _mapper.Map<IEnumerable<SubtitleReference>>(oldLesson.Video.Subtitles.Except(subtitlesDiff));

                var updatedLessonSubtitles = new List<SubtitleReference>(diffSubtitleReferences);

                await _subtitlesManager.DeleteRangeAsync(subtitlesDiffIds, cancellationToken);

                var subtitlesToCreate = lessonAddOrUpdateRequest.Video.Subtitles.Where(l =>
                    !oldLesson.Video.Subtitles.Select(s => s.Language).Contains(l)).Select(l => new Subtitle()
                {
                    Language = l
                });

                var createdSubtitles =
                    await _psqUnitOfWork.SubtitlesRepository.CreateRangeAsync(subtitlesToCreate, cancellationToken);

                var newSubtitleReferences = _mapper.Map<IEnumerable<SubtitleReference>>(createdSubtitles);
                updatedLessonSubtitles.AddRange(newSubtitleReferences);
                updatedLesson.Video.Subtitles = updatedLessonSubtitles;

                var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(lessonAddOrUpdateRequest.Video
                    .Attachment
                    .FileId, cancellationToken);

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
                    PrimaryLanguage = lessonAddOrUpdateRequest.Video.PrimaryLanguage.ToString()
                };

                await _publishEndpoint.Publish(generateRequest, cancellationToken);
            }

            updatedLesson = await _mongoUnitOfWork.Lessons.UpdateAsync(updatedLesson, cancellationToken);
            ts.Complete();
        }

        var response = await GetUpdateResponseAsync(updatedLesson, cancellationToken);

        return ApiResponse<LessonUpdateResponse>.Success(response);
    }

    public async Task<ApiResponse<LessonUpdateResponse>> SaveDraftAsync(
        LessonAddOrUpdateRequest lessonAddOrUpdateRequest, int userId, CancellationToken cancellationToken = default)
    {
        if (lessonAddOrUpdateRequest.Id is null)
            return await CreateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);

        if (lessonAddOrUpdateRequest.EditedLessonId is not null)
        {
            lessonAddOrUpdateRequest.Id = lessonAddOrUpdateRequest.EditedLessonId;
            lessonAddOrUpdateRequest.EditedLessonId = null;

            return await UpdateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);
        }

        var originalLesson =
            await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonAddOrUpdateRequest.Id, cancellationToken);

        if (originalLesson.IsDraft) return await UpdateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);

        lessonAddOrUpdateRequest.Id = null;

        var draft = await CreateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);

        if (!draft.IsSuccess)
            return draft;

        originalLesson.EditedLessonId = draft.Data.Id;
        originalLesson.IsDraft = false;

        await _mongoUnitOfWork.Lessons.UpdateAsync(originalLesson, cancellationToken);

        var response = await GetUpdateResponseAsync(originalLesson, cancellationToken);

        return ApiResponse<LessonUpdateResponse>.Success(response);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonResponse>> GetByIdAsync(string id, int userId,
        CancellationToken cancellationToken = default)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(id, cancellationToken);

        if (lesson is null)
            return ApiResponse<LessonResponse>.Failure(
                new KeyNotFoundException("Cannot find lesson with such Id"));

        var response = _mapper.Map<LessonResponse>(lesson);

        return ApiResponse<LessonResponse>.Success(response);
    }

    private async Task<IEnumerable<SubtitleReference>> AddNewVideoAsync(VideoAddOrUpdateRequest videoAddOrUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(videoAddOrUpdateRequest.Attachment
            .FileId, cancellationToken);
        var subtitlesCreateRequest = videoAddOrUpdateRequest.Subtitles.Select(s => new Subtitle
        {
            Language = s
        });
        var subtitles =
            await _psqUnitOfWork.SubtitlesRepository.CreateRangeAsync(subtitlesCreateRequest, cancellationToken);

        var subtitlesInfo = _mapper.Map<IEnumerable<SubtitleInfo>>(subtitles);

        var subtitlesGenerateRequest = new SubtitlesGenerateRequest
        {
            VideoBlobName = file.BlobName,
            VideoContainerName = file.ContainerName,
            SubtitleInfo = subtitlesInfo,
            PrimaryLanguage = videoAddOrUpdateRequest.PrimaryLanguage.ToString()
        };

        await _publishEndpoint.Publish(subtitlesGenerateRequest, cancellationToken);

        var result = _mapper.Map<IEnumerable<SubtitleReference>>(subtitles);

        return result;
    }

    private async Task<LessonResponse> GetResponseAsync(Lesson lesson, CancellationToken cancellationToken = default)
    {
        var lessonResponse = _mapper.Map<LessonResponse>(lesson);

        lessonResponse.Video.Subtitles = await GetSubtitlesForLessonAsync(lesson, cancellationToken);

        return lessonResponse;
    }

    private async Task<LessonUpdateResponse> GetUpdateResponseAsync(Lesson lesson,
        CancellationToken cancellationToken = default)
    {
        var lessonResponse = _mapper.Map<LessonUpdateResponse>(lesson);

        if (lessonResponse.Video is not null)
            lessonResponse.Video.Subtitles = await GetSubtitlesForLessonAsync(lesson, cancellationToken);

        return lessonResponse;
    }

    private async Task<IEnumerable<SubtitlesResponse>> GetSubtitlesForLessonAsync(Lesson lesson,
        CancellationToken cancellationToken = default)
    {
        var subtitleIds = lesson.Video.Subtitles.Select(s => s.SubtitleId);

        var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdsAsync(subtitleIds, cancellationToken);
        var subtitleResponses = _mapper.Map<IEnumerable<SubtitlesResponse>>(subtitles);

        return subtitleResponses;
    }
}