using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class AnswersService : IAnswersService
{
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly ILessonService _lessonService;
    private readonly IMapper _mapper;

    public AnswersService(IMongoUnitOfWork mongoUnitOfWork, IMapper mapper, ILessonService lessonService)
    {
        _mongoUnitOfWork = mongoUnitOfWork;
        _mapper = mapper;
        _lessonService = lessonService;
    }

    public async Task<AnswersUpdatedResponse> AddOrUpdateAnswersAsync(int userId,
        AnswerAddOrUpdateRequest answerAddOrUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var lessonId = await _lessonService.GetLessonToUpdateIdAsync(answerAddOrUpdateRequest.LessonId, userId,
            cancellationToken: cancellationToken);

        var answer = _mapper.Map<Answers>(answerAddOrUpdateRequest);

        var result = await _mongoUnitOfWork.Answers.AddOrUpdateAnswerAsync(lessonId,
            answerAddOrUpdateRequest.QuizId, answer, cancellationToken: cancellationToken);

        var answerUpdateResponse = _mapper.Map<AnswersUpdatedResponse>(result);

        answerUpdateResponse.CurrentLessonUpdated = new()
        {
            LessonId = lessonId,
        };

        return answerUpdateResponse;
    }
}