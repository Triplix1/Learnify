using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly ILessonService _lessonService;
    private readonly IUserValidatorManager _userValidatorManager;
    private readonly IMapper _mapper;

    public QuizService(IQuizRepository quizRepository, IMapper mapper, ILessonService lessonService, IUserValidatorManager userValidatorManager)
    {
        _quizRepository = quizRepository;
        _mapper = mapper;
        _lessonService = lessonService;
        _userValidatorManager = userValidatorManager;
    }

    public async Task<QuizQuestionUpdateResponse> AddOrUpdateQuizAsync(QuizQuestionAddOrUpdateRequest request,
        int userId,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.LessonId))
            await _userValidatorManager.ValidateAuthorOfLessonAsync(request.LessonId, userId, cancellationToken);
        
        var lessonToUpdateId = await _lessonService.GetLessonToUpdateIdAsync(request.LessonId, userId, cancellationToken);

        QuizQuestion quizQuestion = _mapper.Map<QuizQuestion>(request);

        if (request.QuizId is null)
            quizQuestion = await _quizRepository.CreateAsync(quizQuestion, lessonToUpdateId, cancellationToken);
        else
            quizQuestion = await _quizRepository.UpdateAsync(quizQuestion, lessonToUpdateId, cancellationToken);

        if (quizQuestion is null)
        {
            throw new Exception("Failed to add or update quiz");
        }

        var response = _mapper.Map<QuizQuestionUpdateResponse>(quizQuestion);

        return response;
    }

    public async Task DeleteQuizAsync(string quizId, string lessonId, int userId, CancellationToken cancellationToken = default)
    {
        await _userValidatorManager.ValidateAuthorOfLessonAsync(lessonId, userId, cancellationToken);
        
        var lessonToUpdateId = await _lessonService.GetLessonToUpdateIdAsync(lessonId, userId, cancellationToken);
        
        var deletionResult = await _quizRepository.DeleteAsync(quizId, lessonToUpdateId, cancellationToken);

        if (!deletionResult)
        {
            throw new Exception("Failed to delete quiz");
        }
    }
}