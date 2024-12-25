using AutoMapper;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class QuizService: IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;

    public QuizService(IQuizRepository quizRepository, IMapper mapper)
    {
        _quizRepository = quizRepository;
        _mapper = mapper;
    }

    public async Task<QuizQuestionUpdateResponse> AddOrUpdateQuizAsync(QuizQuestionAddOrUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        QuizQuestion quizQuestion = _mapper.Map<QuizQuestion>(request);
        
        if(request.QuizId is null)
            quizQuestion = await _quizRepository.CreateAsync(quizQuestion, request.LessonId, cancellationToken);
        else
            quizQuestion = await _quizRepository.UpdateAsync(quizQuestion, request.LessonId, cancellationToken);

        if (quizQuestion is null)
        {
            throw new Exception("Failed to add or update quiz");
        }
        
        var response = _mapper.Map<QuizQuestionUpdateResponse>(quizQuestion);
        
        return response;
    }

    public async Task DeleteQuizAsync(string quizId, string lessonId, CancellationToken cancellationToken = default)
    {
        var deletionResult = await _quizRepository.DeleteAsync(quizId, lessonId, cancellationToken);

        if (!deletionResult)
        {
            throw new Exception("Failed to delete quiz");
        }
    }
}