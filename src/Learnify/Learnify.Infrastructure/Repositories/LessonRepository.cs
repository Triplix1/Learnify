using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.ManagerContracts;
using Learnify.Infrastructure.Data;
using Learnify.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class LessonRepository : ILessonRepository
{
    private readonly IMongoAppDbContext _mongoContext;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="LessonRepository"/>
    /// </summary>
    /// <param name="mongoContext"><see cref="IMongoAppDbContext"/></param>
    /// <param name="mapper"><see cref="IMapper"/></param>
    public LessonRepository(IMongoAppDbContext mongoContext, IMapper mapper, ApplicationDbContext context)
    {
        _mongoContext = mongoContext;
        _mapper = mapper;
        _context = context;
    }

    /// <inheritdoc />
    public async Task<LessonUpdateResponse> GetLessonForUpdateAsync(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync();

        if (lesson is null)
            return null;

        var lessonToUpdate = _mapper.Map<LessonUpdateResponse>(lesson);
        
        return lessonToUpdate;
    }

    /// <inheritdoc />
    public async Task<LessonResponse> GetLessonByIdAsync(string id)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, id);

        var lesson = await _mongoContext.Lessons.Find(filter).FirstOrDefaultAsync();
        
        if (lesson is null)
                    return null;
        
        var lessonResponse = _mapper.Map<LessonResponse>(lesson);

        var subtitles = _context.Subtitles.Where(s => lesson.Video.Subtitles.Any(su => su.SubtitleId == s.Id));
        var subtitleResponses = await _mapper.ProjectTo<SubtitlesResponse>(subtitles).ToArrayAsync();
        lessonResponse.Video.Subtitles = subtitleResponses;
        
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

        var projection = Builders<Lesson>.Projection.Include(l => l.Video)
            .Include(l => l.Quizzes);

        var lesson = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).FirstOrDefaultAsync();

        if (lesson is null)
            return Enumerable.Empty<Attachment>();

        var result = new List<Attachment>();

        if (lesson.Video is not null)
            result.Add(lesson.Video.Attachment);

        result.AddRange(lesson.Quizzes.Select(q => q.Media));

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
        var projection = Builders<Lesson>.Projection.Include(l => l.Video)
            .Include(l => l.Quizzes);

        var lessons = await _mongoContext.Lessons.Find(filter).Project<Lesson>(projection).ToListAsync();

        var result = new List<Attachment>();
        result.AddRange(lessons.Select(l => l.Video.Attachment));
        result.AddRange(lessons.SelectMany(l => l.Quizzes.Select(q => q.Media)));

        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetParagraphIdForLessonAsync(string lessonId)
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

        return await GetResponseAsync(lessonCreateRequest);
    }

    /// <inheritdoc />
    public async Task<LessonResponse> UpdateAsync(Lesson lessonUpdateRequest)
    {
        await _mongoContext.Lessons.ReplaceOneAsync(l => l.Id == lessonUpdateRequest.Id, lessonUpdateRequest);

        var updatedLesson = await _mongoContext.Lessons
            .Find(Builders<Lesson>.Filter.Eq(l => l.Id, lessonUpdateRequest.Id)).FirstOrDefaultAsync();
        
        return _mapper.Map<LessonResponse>(updatedLesson);
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

    private async Task<LessonResponse> GetResponseAsync(Lesson lesson)
    {
        var lessonResponse = _mapper.Map<LessonResponse>(lesson);

        var subtitles = _context.Subtitles.Where(s => lesson.Video.Subtitles.Any(su => su.SubtitleId == s.Id));
        var subtitleResponses = await _mapper.ProjectTo<SubtitlesResponse>(subtitles).ToArrayAsync();
        lessonResponse.Video.Subtitles = subtitleResponses;

        return lessonResponse;
    }
}