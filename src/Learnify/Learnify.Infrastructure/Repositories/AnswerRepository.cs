using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

public class AnswerRepository: IAnswerRepository
{
    private readonly IMongoAppDbContext _context;

    public AnswerRepository(IMongoAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetCorrectAnswerAsync(string lessonId, string quizId, CancellationToken cancellationToken = default)
    {
        // Filter to match the specific lesson by ID and quiz by quizId
        var filter = Builders<Lesson>.Filter.And(
            Builders<Lesson>.Filter.Eq(lesson => lesson.Id, lessonId),
            Builders<Lesson>.Filter.ElemMatch(lesson => lesson.Quizzes, quiz => quiz.Id == quizId)
        );

        // Use MongoDB projection to retrieve only the CorrectAnswer field
        var projection = Builders<Lesson>.Projection.Include("Quizzes.Answers.CorrectAnswer");

        // Execute the query
        var lesson = await _context.Lessons
            .Find(filter)
            .Project<Lesson>(projection)
            .FirstOrDefaultAsync(cancellationToken);

        // Extract CorrectAnswer value
        var correctAnswer = lesson?.Quizzes
            .FirstOrDefault(q => q.Id == quizId)?
            .Answers.CorrectAnswer;

        if (correctAnswer == null)
        {
            throw new Exception("Cannot find correct answer for this quiz :)");
        }
        
        return correctAnswer.Value;
    }

    public async Task<Answers> AddOrUpdateAnswerAsync(string lessonId, string quizId, Answers answers, CancellationToken cancellationToken = default)
    {
        // Filter to match the specific lesson by ID
        var filter = Builders<Lesson>.Filter.And(
            Builders<Lesson>.Filter.Eq(lesson => lesson.Id, lessonId),
            Builders<Lesson>.Filter.ElemMatch(lesson => lesson.Quizzes, quiz => quiz.Id == quizId)
        );

        // Update definition to replace the Answers entity of the matched quiz
        var update = Builders<Lesson>.Update.Set(
            lesson => lesson.Quizzes[0].Answers, answers
        );

        var options = new FindOneAndUpdateOptions<Lesson>
        {
            Projection = Builders<Lesson>.Projection.ElemMatch(l => l.Quizzes, q => q.Id == quizId),
            ReturnDocument = ReturnDocument.After
        };

        // Perform the update and retrieve the updated lesson
        var updatedLesson = await _context.Lessons.FindOneAndUpdateAsync(filter, update, options, cancellationToken);

        if (updatedLesson == null)
        {
            throw new Exception("Cannot find lesson with such id");
        }
    
        if (updatedLesson.Quizzes == null || updatedLesson.Quizzes.Count == 0)
        {
            throw new Exception("Cannot find quiz with such id");
        }

        // Extract and return the updated quiz
        return updatedLesson?.Quizzes?.FirstOrDefault(quiz => quiz.Id == quizId)?.Answers;
    }
}