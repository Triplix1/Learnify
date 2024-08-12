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

        var projection = Builders<Lesson>.Projection.Include(l => l.Attachments).Include(l => l.Video);

        var lesson = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).FirstOrDefaultAsync();

        if (lesson is null)
            return Enumerable.Empty<Attachment>();

        var result = new List<Attachment>(lesson.Attachments);
        
        if (lesson.Video is not null)
            result.Add(lesson.Video);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Attachment>> GetAllAttachmentsForLessonsAsync(IEnumerable<string> lessonIds)
    {
        var filter = Builders<Lesson>.Filter.Where(l => lessonIds.Contains(l.Id));

        var projection = Builders<Lesson>.Projection.Include(l => l.Attachments).Include(l => l.Video);

        var lessons = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).ToListAsync();

        var result = new List<Attachment>(lessons.SelectMany(l => l.Attachments));
        result.AddRange(lessons.Select(l => l.Video));
        
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

        var projection = Builders<Lesson>.Projection.Include(l => l.Id);

        var lessonToDelete = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).FirstOrDefaultAsync();

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