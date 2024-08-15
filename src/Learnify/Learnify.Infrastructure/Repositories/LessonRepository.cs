using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.ManagerContracts;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class LessonRepository : ILessonRepository
{
    private readonly IMongoAppDbContext _mongoContext;
    private readonly IMapper _mapper;
    private readonly IBlobStorage _blobStorage;

    /// <summary>
    /// Initializes a new instance of <see cref="LessonRepository"/>
    /// </summary>
    /// <param name="mongoContext"><see cref="IMongoAppDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    /// <param name="blobStorage"><see cref="IBlobStorage"/></param>
    public LessonRepository(IMongoAppDbContext mongoContext, IMapper mapper, IBlobStorage blobStorage)
    {
        _mongoContext = mongoContext;
        _mapper = mapper;
        _blobStorage = blobStorage;
    }

    /// <inheritdoc />
    public async Task<LessonUpdateResponse> GetLessonForUpdate(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync();

        if (lesson is null)
            return null;

        var lessonToUpdate = _mapper.Map<LessonUpdateResponse>(lesson);

        var attachments = new List<AttachmentCreatedResponse>(lessonToUpdate.Attachments);

        if (lesson.Video is not null)
            attachments.Add(lessonToUpdate.Video);

        attachments.AddRange(lessonToUpdate.Quizzes.Select(q => q.Media));

        foreach (var attachmentCreatedResponse in attachments)
        {
            attachmentCreatedResponse.FileUrl = await _blobStorage.GetFileUrlAsync(attachmentCreatedResponse.FileUrl,
                attachmentCreatedResponse.FileBlobName);
        }

        return lessonToUpdate;
    }

    /// <inheritdoc />
    public async Task<LessonResponse> GetLessonById(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync();

        if (lesson is null)
            return null;

        var attachments = new List<Attachment>(lesson.Attachments);

        if (lesson.Video is not null)
            attachments.Add(lesson.Video);

        attachments.AddRange(lesson.Quizzes.Select(q => q.Media));

        var lessonResponse = _mapper.Map<LessonResponse>(lesson);
        
        if (lessonResponse.Video is not null)
            attachmentResponses.Add(lessonResponse.Video);

        attachmentResponses.AddRange(lessonResponse.Quizzes.Select(q => q.Media));
        attachmentResponses.AddRange(lessonResponse.SubtitlesList.Select(q => q.File));

        foreach (var attachmentResponse in attachmentResponses)
        {
            var attachment = attachments.First(a => a.Id == attachmentResponse.Id);

            attachmentResponse.FileUrl =
                await _blobStorage.GetFileUrlAsync(attachment.FileContainerName, attachment.FileBlobName);
        }

        return lessonResponse;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LessonTitleResponse>> GetLessonsForParagraphAsync(int paragraphId)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        var projection =
            Builders<Lesson>.Projection.Expression(l => new LessonTitleResponse { Id = l.Id, Title = l.Title });

        return await _mongoContext.Lessons.Find(filter).Project(projection).ToListAsync();
        ;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonAsync(string lessonId)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        var projection = Builders<Lesson>.Projection.Include(l => l.Attachments).Include(l => l.Video)
            .Include(l => l.Quizzes).Include(l => l.Subtitles);

        var lesson = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).FirstOrDefaultAsync();

        if (lesson is null)
            return Enumerable.Empty<Attachment>();

        var result = new List<Attachment>(lesson.Attachments);

        if (lesson.Video is not null)
            result.Add(lesson.Video);

        result.AddRange(lesson.Quizzes.Select(q => q.Media));
        result.AddRange(lesson.Subtitles.Select(q => q.File));

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphAsync(int paragraphId)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        return await GetAttachmentsForFilterAsync(filter);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphsAsync(IEnumerable<int> paragraphsId)
    {
        var filter = Builders<Lesson>.Filter.Where(l => paragraphsId.Contains(l.ParagraphId));

        return await GetAttachmentsForFilterAsync(filter);
    }

    private async Task<IEnumerable<Attachment>> GetAttachmentsForFilterAsync(FilterDefinition<Lesson> filter)
    {
        var projection = Builders<Lesson>.Projection.Include(l => l.Attachments).Include(l => l.Video)
            .Include(l => l.Quizzes).Include(l => l.Subtitles);

        var lessons = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).ToListAsync();

        var result = new List<Attachment>(lessons.SelectMany(l => l.Attachments));
        result.AddRange(lessons.Select(l => l.Video));
        result.AddRange(lessons.SelectMany(l => l.Quizzes.Select(q => q.Media)));
        result.AddRange(lessons.SelectMany(l => l.Subtitles.Select(q => q.File)));

        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetParagraphIdForLesson(string lessonId)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        var projection = Builders<Lesson>.Projection.Expression(l => new { l.ParagraphId });

        var result = await _mongoContext.Lessons.Find(filter).Project(projection).FirstOrDefaultAsync();

        return result.ParagraphId;
    }

    /// <inheritdoc />
    public async Task<LessonResponse> CreateAsync(Lesson lessonCreateRequest)
    {
        await _mongoContext.Lessons.InsertOneAsync(lessonCreateRequest);

        var lessonResponse = _mapper.Map<LessonResponse>(lessonCreateRequest);

        return lessonResponse;
    }

    /// <inheritdoc />
    public async Task<LessonResponse> UpdateAsync(Lesson lessonUpdateRequest)
    {
        await _mongoContext.Lessons.ReplaceOneAsync(l => l.Id == lessonUpdateRequest.Id, lessonUpdateRequest);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var result = await _mongoContext.Lessons.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    /// <inheritdoc />
    public async Task<long> DeleteForParagraphAsync(int paragraphId)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        var result = await _mongoContext.Lessons.DeleteOneAsync(filter);

        return result.DeletedCount;
    }

    /// <inheritdoc />
    public async Task<long> DeleteForParagraphsAsync(IEnumerable<int> paragraphIds)
    {
        var filter = Builders<Lesson>.Filter.Where(l => paragraphIds.Contains(l.ParagraphId));

        var result = await _mongoContext.Lessons.DeleteManyAsync(filter);

        return result.DeletedCount;
    }
}