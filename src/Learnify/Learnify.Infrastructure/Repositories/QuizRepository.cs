﻿using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

public class QuizRepository: IQuizRepository
{
    private readonly IMongoAppDbContext _context;

    public QuizRepository(IMongoAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuizQuestion>> GetQuizzesByLessonIdAsync(string lessonId, CancellationToken cancellationToken = default)
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
    
    public async Task<QuizQuestion> CreateAsync(QuizQuestion quiz, string lessonId,
        CancellationToken cancellationToken = default)
    {
        // Fetch the existing lesson from MongoDB
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);
        var update = Builders<Lesson>.Update.Push(l => l.Quizzes, quiz);

        // Execute the update and return the updated document
        var options = new FindOneAndUpdateOptions<Lesson>
        {
            Projection = Builders<Lesson>.Projection.Include(l => l.Quizzes), // Project only the Quizzes array
            ReturnDocument = ReturnDocument.After // Return the updated document
        };

        var updatedLesson = await _context.Lessons.FindOneAndUpdateAsync(filter, update, options, cancellationToken);

        // Extract the newly added quiz (or check the whole array if needed)
        var addedQuiz = updatedLesson?.Quizzes?.FirstOrDefault(q => q.Id == quiz.Id);

        return addedQuiz;
    }

    public async Task<QuizQuestion> UpdateAsync(QuizQuestion quiz, string lessonId,
        CancellationToken cancellationToken = default)
    {
        // Fetch the existing lesson from MongoDB
        var filter = Builders<Lesson>.Filter.Eq(l => l.Id, lessonId);

        var update = Builders<Lesson>.Update.Set("Quizzes.$[quiz]", quiz);

        var options = new FindOneAndUpdateOptions<Lesson>
        {
            Projection = Builders<Lesson>.Projection.ElemMatch(l => l.Quizzes, q => q.Id == quiz.Id),
            ReturnDocument = ReturnDocument.After
        };

        var updatedLesson = await _context.Lessons.FindOneAndUpdateAsync(filter, update, options, cancellationToken);

        var result = updatedLesson?.Quizzes?.FirstOrDefault();

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