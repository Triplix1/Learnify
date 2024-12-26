using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class QuizController : BaseApiController
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<QuizQuestionUpdateResponse>> AddOrUpdateQuizAsync(
        QuizQuestionAddOrUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var response = await _quizService.AddOrUpdateQuizAsync(request, userId, cancellationToken);

        return Ok(response);
    }

    [Authorize]
    [HttpPost("{lessonId}/{quizId}")]
    public async Task<ActionResult> DeleteQuizAsync(string lessonId, string quizId,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        await _quizService.DeleteQuizAsync(quizId, lessonId, userId, cancellationToken);

        return Ok();
    }
}