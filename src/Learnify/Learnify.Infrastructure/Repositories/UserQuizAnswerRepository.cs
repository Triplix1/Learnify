using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class UserQuizAnswerRepository: IUserQuizAnswerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;

    public UserQuizAnswerRepository(ApplicationDbContext context,
        IQuizRepository quizRepository,
        IMapper mapper)
    {
        _context = context;
        _quizRepository = quizRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserQuizAnswerResponse>> GetUserQuizAnswersForLessonAsync(int userId, string lessonId, CancellationToken cancellationToken = default)
    {
        var userQuizAnswers = await _context.UserQuizAnswers.Where(a => a.UserId == userId && a.LessonId == lessonId)
            .ToArrayAsync(cancellationToken: cancellationToken);

        return await GetQuizAnswerResponsesForUserQuizzesAsync(lessonId, userQuizAnswers, cancellationToken);
    }

    public async Task<IEnumerable<UserQuizAnswerResponse>> SaveUserAnswersAsync(int userId, string lessonId, IEnumerable<UserQuizAnswerCreateRequest> userQuizAnswerCreateRequests, CancellationToken cancellationToken = default)
    {
        var createdEntities = new List<UserQuizAnswer>();
        var updatedEntities = new List<UserQuizAnswer>();
        
        foreach (var userQuizAnswerCreateRequest in userQuizAnswerCreateRequests)
        {
            var existingEntity = await _context.UserQuizAnswers.FindAsync(
            [
                userId, 
                lessonId,
                userQuizAnswerCreateRequest.QuizId
            ], cancellationToken);

            if (existingEntity == null)
            {
                existingEntity = _mapper.Map<UserQuizAnswer>(userQuizAnswerCreateRequest);
                existingEntity.LessonId = lessonId;
                existingEntity.UserId = userId;
                createdEntities.Add(existingEntity);
            }
            else
            {
                _mapper.Map(userQuizAnswerCreateRequest, existingEntity);
                updatedEntities.Add(existingEntity);
            }
        }
        
        _context.UserQuizAnswers.AddRange(createdEntities);
        _context.UserQuizAnswers.UpdateRange(updatedEntities);
        await _context.SaveChangesAsync(cancellationToken);
        
        updatedEntities.AddRange(createdEntities);
        
        return await GetQuizAnswerResponsesForUserQuizzesAsync(lessonId, updatedEntities, cancellationToken);
    }

    public async Task RemoveByQuizIdsAsync(IEnumerable<string> quizIds, CancellationToken cancellationToken = default)
    {
        var quizzes = await _context.UserQuizAnswers.Where(a => quizIds.Contains(a.QuizId))
            .ToArrayAsync(cancellationToken);
        
        _context.UserQuizAnswers.RemoveRange(quizzes);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<UserQuizAnswerResponse>> GetQuizAnswerResponsesForUserQuizzesAsync(
        string lessonId, IList<UserQuizAnswer> userQuizAnswers, CancellationToken cancellationToken = default)
    {
        var quizzes = await _quizRepository.GetQuizzesByLessonIdAsync(lessonId, cancellationToken);
        
        var response = new UserQuizAnswerResponse[userQuizAnswers.Count];

        for (int i = 0; i < userQuizAnswers.Count; i++)
        {
            var isCorrect = quizzes.SingleOrDefault(q => q.Id == userQuizAnswers[i].QuizId)?.Answers.CorrectAnswer ==
                            userQuizAnswers[i].AnswerIndex;

            response[i] = new UserQuizAnswerResponse
            {
                QuizId = userQuizAnswers[i].QuizId,
                AnswerIndex = userQuizAnswers[i].AnswerIndex,
                IsCorrect = isCorrect,
            };
        }

        return response;

    }
}