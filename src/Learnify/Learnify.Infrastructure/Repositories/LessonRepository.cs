using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class LessonRepository : ILessonRepository
{
    private readonly IMongoAppDbContext _mongoContext;

    /// <summary>
    /// Initializes a new instance of <see cref="LessonRepository"/>
    /// </summary>
    /// <param name="mongoContext"><see cref="IMongoAppDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    public LessonRepository(IMongoAppDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<string> GetLessonToUpdateIdForCurrentLessonAsync(string lessonId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(x => x.Id, lessonId);
        var projection = Builders<Lesson>.Projection.Include(x => x.IsDraft).Include(x => x.EditedLessonId);

        var lesson = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection)
            .FirstOrDefaultAsync(cancellationToken);

        var lessonToUpdateId = lesson.IsDraft ? lesson.Id : lesson.EditedLessonId;

        return lessonToUpdateId;
    }

    /// <inheritdoc />
    public async Task<Lesson> GetLessonByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return lesson;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LessonTitleResponse>> GetLessonTitlesForParagraphAsync(int paragraphId,
        bool includeDrafts, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        if (!includeDrafts)
        {
            var includeDraftsFilter = Builders<Lesson>.Filter.Where(l => !l.IsDraft);
            filter = Builders<Lesson>.Filter.And(filter, includeDraftsFilter);
        }

        var projection =
            Builders<Lesson>.Projection.Expression(l => new LessonTitleResponse { Id = l.Id, Title = l.Title });

        return await _mongoContext.Lessons.Find(filter).Project(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonAsync(string lessonId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        var projection = Builders<Lesson>.Projection.Include(l => l.Video)
            .Include(l => l.Quizzes);

        var lesson = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (lesson is null)
            return [];

        var result = new List<Attachment>();

        if (lesson.Video is not null)
            result.Add(lesson.Video.Attachment);

        result.AddRange(lesson.Quizzes.Where(q => q.Media is not null).Select(q => q.Media));

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphAsync(int paragraphId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        return await GetAttachmentsForFilterAsync(filter, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForParagraphsAsync(IEnumerable<int> paragraphsId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Where(l => paragraphsId.Contains(l.ParagraphId));

        return await GetAttachmentsForFilterAsync(filter, cancellationToken);
    }

    private async Task<IEnumerable<Attachment>> GetAttachmentsForFilterAsync(FilterDefinition<Lesson> filter,
        CancellationToken cancellationToken = default)
    {
        var projection = Builders<Lesson>.Projection.Include(l => l.Video)
            .Include(l => l.Quizzes);

        var lessons = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection)
            .ToListAsync(cancellationToken: cancellationToken);

        var result = new List<Attachment>();
        result.AddRange(lessons.Select(l => l.Video.Attachment));
        result.AddRange(lessons.SelectMany(l => l.Quizzes.Select(q => q.Media)));

        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetParagraphIdForLessonAsync(string lessonId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        var projection = Builders<Lesson>.Projection.Expression(l => new { l.ParagraphId });

        var result = await _mongoContext.Lessons.Find(filter).Project(projection)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return result.ParagraphId;
    }

    /// <inheritdoc />
    public async Task<Lesson> CreateAsync(Lesson lessonCreateRequest, CancellationToken cancellationToken = default)
    {
        await _mongoContext.Lessons.InsertOneAsync(lessonCreateRequest, cancellationToken: cancellationToken);

        return lessonCreateRequest;
    }

    /// <inheritdoc />
    public async Task<Lesson> UpdateAsync(Lesson lessonUpdateRequest, CancellationToken cancellationToken = default)
    {
        await _mongoContext.Lessons.ReplaceOneAsync(l => l.Id == lessonUpdateRequest.Id, lessonUpdateRequest,
            cancellationToken: cancellationToken);

        var updatedLesson = await _mongoContext.Lessons
            .Find(Builders<Lesson>.Filter.Eq(l => l.Id, lessonUpdateRequest.Id))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return lessonUpdateRequest;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var result = await _mongoContext.Lessons.DeleteOneAsync(filter, cancellationToken);

        return result.DeletedCount > 0;
    }

    /// <inheritdoc />
    public async Task<long> DeleteForParagraphAsync(int paragraphId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        var result = await _mongoContext.Lessons.DeleteOneAsync(filter, cancellationToken);

        return result.DeletedCount;
    }

    /// <inheritdoc />
    public async Task<long> DeleteForParagraphsAsync(IEnumerable<int> paragraphIds,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Where(l => paragraphIds.Contains(l.ParagraphId));

        var result = await _mongoContext.Lessons.DeleteManyAsync(filter, cancellationToken);

        return result.DeletedCount;
    }
}