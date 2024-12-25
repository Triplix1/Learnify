using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class QuizController: BaseApiController
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<QuizQuestionUpdateResponse>>> AddOrUpdateQuizAsync(
        QuizQuestionAddOrUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _quizService.AddOrUpdateQuizAsync(request, cancellationToken);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteQuizAsync(string quizId, string lessonId,
        CancellationToken cancellationToken = default)
    {
        var response = await _quizService.DeleteQuizAsync(quizId, lessonId, cancellationToken);
        
        return Ok(response);
    }
}