﻿using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;
using MongoDB.Bson;

namespace Learnify.Core.Services;

public class LessonService : ILessonService
{
    private readonly ISubtitlesManager _subtitlesManager;
    private readonly IBlobStorage _blobStorage;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly ISummaryManager _summaryManager;

    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMapper _mapper;

    public LessonService(IMongoUnitOfWork mongoUnitOfWork,
        IPsqUnitOfWork psqUnitOfWork,
        IMapper mapper,
        ISubtitlesManager subtitlesManager,
        IBlobStorage blobStorage,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        ISummaryManager summaryManager)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _subtitlesManager = subtitlesManager;
        _blobStorage = blobStorage;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _summaryManager = summaryManager;
    }

    #region DQL

    public async Task<LessonResponse> GetByIdAsync(string id, int userId,
        CancellationToken cancellationToken = default)
    {
        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(id, cancellationToken);

        if (lesson is null)
            throw new KeyNotFoundException("Cannot find lesson with such Id");

        var response = _mapper.Map<LessonResponse>(lesson);

        response.Video.Subtitles = await GetSubtitlesForLessonAsync(lesson, cancellationToken);

        var userAnswers = await
            _psqUnitOfWork.UserQuizAnswerRepository.GetUserQuizAnswersForLessonAsync(userId, response.Id,
                cancellationToken);

        foreach (var quiz in response.Quizzes)
        {
            var quizAnswer = userAnswers.SingleOrDefault(x => x.QuizId == quiz.Id);

            if (quizAnswer != null)
            {
                quiz.UserAnswer = new UserLessonQuizAnswerResponse()
                {
                    AnswerIndex = quizAnswer.AnswerIndex,
                    IsCorrect = quizAnswer.IsCorrect,
                };
            }
        }

        return response;
    }

    public async Task<string> GetLessonToUpdateIdAsync(string lessonId, int userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(lessonId))
        {
            throw new Exception("You trying to update lesson that doesn't exist");
        }

        await _userAuthorValidatorManager.ValidateAuthorOfLessonAsync(lessonId, userId, cancellationToken);

        var lessonToUpdateId =
            await _mongoUnitOfWork.Lessons.GetLessonToUpdateIdForCurrentLessonAsync(lessonId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(lessonToUpdateId))
            return lessonToUpdateId;

        var originalLesson =
            await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonId, cancellationToken);

        if (originalLesson is null)
            throw new KeyNotFoundException("Cannot find lesson with such id");

        var draft = _mapper.Map<Lesson>(originalLesson);
        draft.Id = default;
        draft.IsDraft = true;
        draft.OriginalLessonId = lessonId;

        draft = await _mongoUnitOfWork.Lessons.CreateAsync(draft, cancellationToken);

        originalLesson.Id = lessonId;
        originalLesson.EditedLessonId = draft.Id;

        await _mongoUnitOfWork.Lessons.UpdateAsync(originalLesson, cancellationToken);

        return draft.Id;
    }

    public async Task<IEnumerable<LessonTitleResponse>> GetByParagraphAsync(int paragraphId, int userId,
        bool includeDrafts = false, CancellationToken cancellationToken = default)
    {
        if (includeDrafts)
        {
            var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(paragraphId, cancellationToken);
            if (userId != authorId)
                throw
                    new Exception("You should be author of the course to be able see draft lessons");
        }

        var response =
            await _mongoUnitOfWork.Lessons.GetLessonTitlesForParagraphAsync(paragraphId, includeDrafts,
                cancellationToken);

        return response;
    }

    /// <inheritdoc />
    public async Task<LessonUpdateResponse> GetForUpdateAsync(string id, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfLessonAsync(id, userId, cancellationToken);

        var editedLessonId =
            await _mongoUnitOfWork.Lessons.GetLessonToUpdateIdForCurrentLessonAsync(id, cancellationToken);

        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(editedLessonId ?? id, cancellationToken);

        if (lesson is null)
            throw
                new KeyNotFoundException("Cannot find lesson with such Id");

        var response = await GetUpdateResponseAsync(lesson, cancellationToken);

        return response;
    }

    #endregion

    #region DML

    public async Task<LessonUpdateResponse> AddOrUpdateAsync(
        LessonAddOrUpdateRequest lessonAddOrUpdateRequest, int userId, CancellationToken cancellationToken = default)
    {
        if (lessonAddOrUpdateRequest.Id is null)
            return await CreateAsync(lessonAddOrUpdateRequest, userId, cancellationToken: cancellationToken);

        return await UpdateAsync(lessonAddOrUpdateRequest, userId, cancellationToken: cancellationToken);
    }

    public async Task<LessonUpdateResponse> SaveDraftAsync(
        LessonAddOrUpdateRequest lessonAddOrUpdateRequest, int userId, CancellationToken cancellationToken = default)
    {
        if (lessonAddOrUpdateRequest.Id is null)
        {
            var createdLesson = await CreateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);

            lessonAddOrUpdateRequest.Id = createdLesson.Id;
        }
        else
        {
            lessonAddOrUpdateRequest.Id =
                await GetLessonToUpdateIdAsync(lessonAddOrUpdateRequest.Id, userId, cancellationToken);
        }

        var updatedLesson = await UpdateAsync(lessonAddOrUpdateRequest, userId, true, cancellationToken);

        return updatedLesson;
    }

    public async Task DeleteAsync(string id, int userId, CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfLessonAsync(id, userId, cancellationToken);

        var lessonToUpdateId =
            await _mongoUnitOfWork.Lessons.GetLessonToUpdateIdForCurrentLessonAsync(id, cancellationToken);

        var fullDelete = lessonToUpdateId != null && lessonToUpdateId != id;

        await SafelyDeleteLessonsAndAttachmentsAsync(id, fullDelete, cancellationToken);
    }

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

    public async Task DeleteLessonsByParagraphs(IEnumerable<int> paragraphIds,
        CancellationToken cancellationToken = default)
    {
        var attachments =
            await _mongoUnitOfWork.Lessons.GetAllAttachmentsForParagraphsAsync(paragraphIds, cancellationToken);

        await _mongoUnitOfWork.Lessons.DeleteForParagraphsAsync(paragraphIds, cancellationToken);
    }

    #endregion

    #region Private

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

        var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdsAsync(subtitleIds, [], cancellationToken);
        var subtitleResponses = _mapper.Map<IEnumerable<SubtitlesResponse>>(subtitles);

        return subtitleResponses;
    }

    private async Task<int> GetCourseIdAsync(string lessonId, CancellationToken cancellationToken = default)
    {
        var paragraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(lessonId, cancellationToken);
        var courseId = await _psqUnitOfWork.ParagraphRepository.GetCourseIdAsync(paragraphId, cancellationToken);

        if (courseId is null)
            throw new KeyNotFoundException("Cannot find course for lesson with id " + lessonId);

        return courseId.Value;
    }

    private async Task<VideoProceedResponse> ProcessNewVideoAsync(int? courseId,
        string lessonId,
        Language primaryLanguage,
        VideoAddOrUpdateRequest videoAddOrUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(videoAddOrUpdateRequest.Attachment
            .FileId, cancellationToken);

        var fileDataBlobResponse = _mapper.Map<PrivateFileDataBlobResponse>(file);

        var subtitlesCreateAndGenerateRequest = new SubtitlesCreateAndGenerateRequest()
        {
            PrivateFileDataBlobResponse = fileDataBlobResponse,
            CourseId = courseId,
            LessonId = lessonId,
            PrimaryLanguage = primaryLanguage,
            SubtitlesLanguages = videoAddOrUpdateRequest.Subtitles
        };

        var subtitles =
            await _subtitlesManager.CreateAndGenerateAsync(subtitlesCreateAndGenerateRequest, cancellationToken);

        var summaryId =
            await _summaryManager.GenerateSummaryForVideoAsync(fileDataBlobResponse, courseId, primaryLanguage);

        return new VideoProceedResponse()
        {
            Subtitles = subtitles,
            SummaryFileId = summaryId,
        };
    }

    private async Task<LessonUpdateResponse> CreateAsync(LessonAddOrUpdateRequest lessonCreateRequest,
        int userId, bool draft = false, CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfParagraphAsync(lessonCreateRequest.ParagraphId, userId,
            cancellationToken: cancellationToken);

        var lesson = _mapper.Map<Lesson>(lessonCreateRequest);
        lesson.IsDraft = draft;

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            lesson.Id = ObjectId.GenerateNewId().ToString();

            if (lessonCreateRequest.Video is not null)
            {
                var courseId =
                    await _psqUnitOfWork.ParagraphRepository.GetCourseIdAsync(lessonCreateRequest.ParagraphId,
                        cancellationToken);

                if (courseId is null)
                    throw new KeyNotFoundException("Cannot find course for provided lesson");

                var videoProceedResponse = await ProcessNewVideoAsync(courseId.Value, lesson.Id,
                    lessonCreateRequest.PrimaryLanguage,
                    lessonCreateRequest.Video, cancellationToken);

                lesson.Video.Subtitles = videoProceedResponse.Subtitles;
                lesson.Video.SummaryFileId = videoProceedResponse.SummaryFileId;
            }

            var createdLesson = await _mongoUnitOfWork.Lessons.CreateAsync(lesson, cancellationToken);

            ts.Complete();

            var response = await GetUpdateResponseAsync(createdLesson, cancellationToken);

            return response;
        }
    }

    private async Task<LessonUpdateResponse> UpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest,
        int userId, bool draft = false, CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfLessonAsync(lessonAddOrUpdateRequest.Id, userId,
            cancellationToken: cancellationToken);

        var oldLesson =
            await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(lessonAddOrUpdateRequest.Id, cancellationToken);

        var updatedLesson = _mapper.Map<Lesson>(oldLesson);

        if (lessonAddOrUpdateRequest.Video is not null && (lessonAddOrUpdateRequest.Video.Subtitles == null ||
                                                           !lessonAddOrUpdateRequest.Video.Subtitles.Any()))
        {
            lessonAddOrUpdateRequest.Video.Subtitles = [lessonAddOrUpdateRequest.PrimaryLanguage];
        }

        updatedLesson = _mapper.Map(lessonAddOrUpdateRequest, updatedLesson);
        updatedLesson.IsDraft = draft;

        var courseId = await GetCourseIdAsync(oldLesson.Id, cancellationToken);

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            if (!draft)
            {
                if (updatedLesson.EditedLessonId is not null)
                {
                    await SafelyDeleteLessonsAndAttachmentsAsync(updatedLesson.EditedLessonId,
                        cancellationToken: cancellationToken);
                    updatedLesson.EditedLessonId = null;
                }

                if (updatedLesson.OriginalLessonId is not null)
                {
                    await SafelyDeleteLessonsAndAttachmentsAsync(updatedLesson.OriginalLessonId,
                        cancellationToken: cancellationToken);
                    updatedLesson.OriginalLessonId = null;
                }
            }

            if (lessonAddOrUpdateRequest.Video?.Attachment.FileId != oldLesson.Video?.Attachment.FileId)
            {
                if (oldLesson.Video is not null)
                {
                    if (oldLesson.OriginalLessonId is not null && !draft)
                    {
                        var originalLesson =
                            await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(oldLesson.OriginalLessonId,
                                cancellationToken);

                        var subtitlesDiff =
                            oldLesson.Video.Subtitles.Except(originalLesson.Video.Subtitles);

                        var subtitlesDiffIds = subtitlesDiff.Select(s => s.SubtitleId);

                        await _subtitlesManager.DeleteRangeAsync(subtitlesDiffIds, cancellationToken);
                    }
                }

                if (lessonAddOrUpdateRequest.Video?.Attachment?.FileId != null)
                {
                    var videoProceedResponse =
                        await ProcessNewVideoAsync(courseId, updatedLesson.Id, lessonAddOrUpdateRequest.PrimaryLanguage,
                            lessonAddOrUpdateRequest.Video, cancellationToken);

                    updatedLesson.Video.Subtitles = videoProceedResponse.Subtitles;
                    updatedLesson.Video.SummaryFileId = videoProceedResponse.SummaryFileId;
                }
            }
            else if (lessonAddOrUpdateRequest.Video?.Attachment is not null && oldLesson.Video?.Attachment is not null)
            {
                var primaryLanguageEquals =
                    oldLesson.PrimaryLanguage == lessonAddOrUpdateRequest.PrimaryLanguage;

                var subtitlesDiff =
                    oldLesson.Video.Subtitles.Where(s =>
                        !primaryLanguageEquals || !lessonAddOrUpdateRequest.Video.Subtitles.Contains(s.Language));

                var subtitlesDiffIds = subtitlesDiff.Select(s => s.SubtitleId);

                if (subtitlesDiffIds.Any())
                {
                    if (!draft)
                        await _subtitlesManager.DeleteRangeAsync(subtitlesDiffIds, cancellationToken);

                    updatedLesson.Video.Subtitles =
                        updatedLesson.Video.Subtitles.Where(s => !subtitlesDiffIds.Contains(s.SubtitleId));
                }

                var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(lessonAddOrUpdateRequest.Video
                    .Attachment
                    .FileId, cancellationToken);

                var fileDataBlobResponse = _mapper.Map<PrivateFileDataBlobResponse>(file);

                var newSubtitlesLanguages = lessonAddOrUpdateRequest.Video.Subtitles.Where(l =>
                    !primaryLanguageEquals || !oldLesson.Video.Subtitles.Select(s => s.Language).Contains(l)).ToArray();

                if (newSubtitlesLanguages.Length > 0)
                {
                    if (!primaryLanguageEquals)
                    {
                        var subtitlesCreateAndGenerateRequest = new SubtitlesCreateAndGenerateRequest()
                        {
                            PrivateFileDataBlobResponse = fileDataBlobResponse,
                            CourseId = courseId,
                            LessonId = updatedLesson.Id,
                            PrimaryLanguage = lessonAddOrUpdateRequest.PrimaryLanguage,
                            SubtitlesLanguages = newSubtitlesLanguages
                        };

                        updatedLesson.Video.Subtitles =
                            await _subtitlesManager.CreateAndGenerateAsync(subtitlesCreateAndGenerateRequest,
                                cancellationToken);
                    }
                    else
                    {
                        var subtitleCreateRequests = newSubtitlesLanguages.Select(s => new SubtitlesCreateRequest()
                        {
                            Language = s
                        });

                        var newSubtitles = await
                            _subtitlesManager.CreateSubtitlesAsync(courseId, subtitleCreateRequests, cancellationToken);

                        var subtitleReferences = _mapper.Map<IEnumerable<SubtitleReference>>(newSubtitles);

                        updatedLesson.Video.Subtitles = updatedLesson.Video.Subtitles.Concat(subtitleReferences);

                        var primaryLanguageSubtitleReference =
                            updatedLesson.Video.Subtitles.Single(s => s.Language == updatedLesson.PrimaryLanguage);

                        var primaryLanguageSubtitleFile =
                            await _psqUnitOfWork.PrivateFileRepository.GetBySubtitleIdAsync(
                                primaryLanguageSubtitleReference.SubtitleId, cancellationToken);

                        // Otherwise it will be translated automatically after receiving primary subtitle
                        if (primaryLanguageSubtitleFile?.ContainerName != null &&
                            primaryLanguageSubtitleFile?.ContainerName != null)
                        {
                            var newSubtitlesIds = newSubtitles.Select(s => s.Id);

                            await _subtitlesManager.RequestSubtitlesTranslationAsync(
                                primaryLanguageSubtitleReference.SubtitleId, newSubtitlesIds, cancellationToken);
                        }
                    }
                }
            }

            updatedLesson = await _mongoUnitOfWork.Lessons.UpdateAsync(updatedLesson, cancellationToken);
            ts.Complete();
        }

        var response = await GetUpdateResponseAsync(updatedLesson, cancellationToken);

        return response;
    }

    private async Task SafelyDeleteLessonsAndAttachmentsAsync(string id, bool totalDelete = false,
        CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var lesson = await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(id, cancellationToken);

        var lessonToDelete = await GetLessonToDeleteResponse(lesson, cancellationToken);

        var relatedLessonId = lessonToDelete.EditedLessonId ?? lessonToDelete.OriginalLessonId;

        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            if (relatedLessonId != null)
            {
                var relatedLesson =
                    await _mongoUnitOfWork.Lessons.GetLessonByIdAsync(relatedLessonId, cancellationToken);

                var relatedLessonToDelete = await GetLessonToDeleteResponse(relatedLesson, cancellationToken);

                if (totalDelete)
                {
                    lessonToDelete.Subtitles =
                        lessonToDelete.Subtitles.Concat(relatedLessonToDelete.Subtitles).Distinct();
                    lessonToDelete.Attachments =
                        lessonToDelete.Attachments.Concat(relatedLessonToDelete.Attachments).Distinct();
                }
                else
                {
                    lessonToDelete.Subtitles = lessonToDelete.Subtitles.Except(relatedLessonToDelete.Subtitles);
                    lessonToDelete.Attachments = lessonToDelete.Attachments.Except(relatedLessonToDelete.Attachments);

                    if (relatedLessonId == lessonToDelete.EditedLessonId)
                    {
                        relatedLesson.IsDraft = false;
                        relatedLesson.OriginalLessonId = null;
                    }
                    else
                    {
                        relatedLesson.EditedLessonId = null;
                    }

                    await _mongoUnitOfWork.Lessons.UpdateAsync(relatedLesson, cancellationToken);
                }
            }

            await _psqUnitOfWork.PrivateFileRepository.DeleteRangeAsync(lessonToDelete.Attachments, cancellationToken);

            await _subtitlesManager.DeleteRangeAsync(lessonToDelete.Subtitles, cancellationToken);

            await _mongoUnitOfWork.Lessons.DeleteAsync(id, cancellationToken);

            if (totalDelete && relatedLessonId != null)
                await _mongoUnitOfWork.Lessons.DeleteAsync(relatedLessonId, cancellationToken);

            ts.Complete();
        }
    }

    private async Task<LessonToDeleteResponse> GetLessonToDeleteResponse(Lesson lesson,
        CancellationToken cancellationToken = default)
    {
        var attachments = await _mongoUnitOfWork.Lessons.GetAllAttachmentsForLessonAsync(lesson.Id, cancellationToken);

        var attachmentFileIds = attachments.Select(a => a.FileId);

        var subtitleIds = lesson.Video?.Subtitles.Select(s => s.SubtitleId) ?? [];

        var result = new LessonToDeleteResponse
        {
            Id = lesson.Id,
            EditedLessonId = lesson.EditedLessonId,
            OriginalLessonId = lesson.OriginalLessonId,
            IsDraft = lesson.IsDraft,
            Attachments = attachmentFileIds,
            Subtitles = subtitleIds,
        };

        return result;
    }

    #endregion
}