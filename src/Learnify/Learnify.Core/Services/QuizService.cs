using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class QuizService : IQuizService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly ILessonService _lessonService;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IUserBoughtValidatorManager _userBoughtValidatorManager;
    private readonly IMapper _mapper;

    public QuizService(IMapper mapper,
        ILessonService lessonService,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IPsqUnitOfWork psqUnitOfWork,
        IUserBoughtValidatorManager userBoughtValidatorManager,
        IMongoUnitOfWork mongoUnitOfWork)
    {
        _mapper = mapper;
        _lessonService = lessonService;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _psqUnitOfWork = psqUnitOfWork;
        _userBoughtValidatorManager = userBoughtValidatorManager;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task<QuizQuestionUpdateResponse> AddOrUpdateQuizAsync(QuizQuestionAddOrUpdateRequest request,
        int userId,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.LessonId))
            await _userAuthorValidatorManager.ValidateAuthorOfLessonAsync(request.LessonId, userId, cancellationToken);

        var lessonToUpdateId =
            await _lessonService.GetLessonToUpdateIdAsync(request.LessonId, userId, cancellationToken);

        QuizQuestion quizQuestion = _mapper.Map<QuizQuestion>(request);

        if (request.QuizId is null)
            quizQuestion =
                await _psqUnitOfWork.QuizRepository.CreateAsync(quizQuestion, lessonToUpdateId, cancellationToken);
        else
            quizQuestion =
                await _psqUnitOfWork.QuizRepository.UpdateAsync(quizQuestion, lessonToUpdateId, cancellationToken);

        if (quizQuestion is null)
        {
            throw new Exception("Failed to add or update quiz");
        }

        var response = _mapper.Map<QuizQuestionUpdateResponse>(quizQuestion);

        return response;
    }

    public async Task DeleteQuizAsync(string quizId, string lessonId, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfLessonAsync(lessonId, userId, cancellationToken);

        var lessonToUpdateId = await _lessonService.GetLessonToUpdateIdAsync(lessonId, userId, cancellationToken);

        var deletionResult =
            await _psqUnitOfWork.QuizRepository.DeleteAsync(quizId, lessonToUpdateId, cancellationToken);

        if (!deletionResult)
        {
            throw new Exception("Failed to delete quiz");
        }
    }

    public async Task<IEnumerable<UserQuizAnswerResponse>> CheckAnswersAsync(AnswersValidateRequest request, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userBoughtValidatorManager.ValidateUserAccessToTheLessonAsync(userId, request.LessonId,
            cancellationToken);

        var quizAnswerSaveRequests = new UserQuizAnswerCreateRequest[request.QuizValidateRequests.Count];

        for (int i = 0; i < request.QuizValidateRequests.Count; i++)
        {
            quizAnswerSaveRequests[i] = new UserQuizAnswerCreateRequest()
            {
                QuizId = request.QuizValidateRequests[i].Id,
                AnswerIndex = request.QuizValidateRequests[i].Answer
            };
        }

        var result = await _psqUnitOfWork.UserQuizAnswerRepository.SaveUserAnswersAsync(userId, request.LessonId,
            quizAnswerSaveRequests, cancellationToken);

        return result;
    }
}