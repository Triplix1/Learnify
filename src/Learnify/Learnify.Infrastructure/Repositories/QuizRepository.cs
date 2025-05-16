using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly IMongoAppDbContext _context;
    private readonly IMapper _mapper;

    public QuizRepository(IMongoAppDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuizQuestion>> GetQuizzesByLessonIdAsync(string lessonId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        // Define the projection to retrieve only the quizzes array
        var projection = Builders<Lesson>.Projection.Include(l => l.Quizzes);

        // Execute the query
        var lessonWithQuizzes = await _context.Lessons.Find(filter)
            .Project<Lesson>(projection)
            .FirstOrDefaultAsync(cancellationToken);

        return lessonWithQuizzes?.Quizzes;
    }

    public async Task<QuizQuestion> CreateAsync(QuizQuestionAddOrUpdateRequest quizRequest,
        CancellationToken cancellationToken = default)
    {
        var quiz = _mapper.Map<QuizQuestion>(quizRequest);
        quiz.Id = ObjectId.GenerateNewId().ToString();
        
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, quizRequest.LessonId);
        var update = Builders<Lesson>.Update.Push(l => l.Quizzes, quiz);

        var options = new FindOneAndUpdateOptions<Lesson>
        {
            Projection = Builders<Lesson>.Projection.Slice(l => l.Quizzes, -1),
            ReturnDocument = ReturnDocument.After
        };

        var updatedLesson = await _context.Lessons.FindOneAndUpdateAsync(filter, update, options, cancellationToken);

        var addedQuiz = updatedLesson?.Quizzes?.FirstOrDefault();

        return addedQuiz;
    }

    public async Task<QuizQuestion> UpdateAsync(QuizQuestionAddOrUpdateRequest quiz,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Lesson>.Filter.And(
            Builders<Lesson>.Filter.Eq(l => l.Id, quiz.LessonId),
            Builders<Lesson>.Filter.ElemMatch(l => l.Quizzes, q => q.Id == quiz.QuizId)
        );

        var update = Builders<Lesson>.Update.Set("Quizzes.$.Question", quiz.Question);

        var options = new FindOneAndUpdateOptions<Lesson>
        {
            ReturnDocument = ReturnDocument.After
        };

        var updatedLesson = await _context.Lessons
            .FindOneAndUpdateAsync(filter, update, options, cancellationToken);

        var result = updatedLesson?.Quizzes?.FirstOrDefault(q => q.Id == quiz.QuizId);

        return result;
    }

    public async Task<bool> DeleteAsync(string id, string lessonId, CancellationToken cancellationToken = default)
    {
        // Fetch the existing lesson from MongoDB
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        // Define the update operation to remove the quiz
        var update = Builders<Lesson>.Update.PullFilter(
            l => l.Quizzes,
            q => q.Id == id
        );

        // Execute the update
        var updateResult = await _context.Lessons.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount == 1;
    }
}