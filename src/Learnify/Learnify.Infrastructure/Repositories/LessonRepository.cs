using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class LessonRepository: ILessonRepository
{
    private readonly IMongoAppDbContext _mongoContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="LessonRepository"/>
    /// </summary>
    /// <param name="mongoContext"><see cref="IMongoAppDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    public LessonRepository(IMongoAppDbContext mongoContext, IMapper mapper)
    {
        _mongoContext = mongoContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<LessonUpdateResponse> GetLessonForUpdate(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync();

        if (lesson is null)
            return null;

        return _mapper.Map<LessonUpdateResponse>(lesson);
    }

    /// <inheritdoc />
    public async Task<LessonResponse> GetLessonById(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync();

        if (lesson is null)
            return null;
        
        return _mapper.Map<LessonResponse>(lesson);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LessonTitleResponse>> GetLessonsForParagraphAsync(int paragraphId)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.ParagraphId, paragraphId);

        var projection = Builders<Lesson>.Projection.Include(l => l.Id).Include(l => l.Title);

        var lessons = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).ToListAsync();

        return _mapper.Map<List<LessonTitleResponse>>(lessons);
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