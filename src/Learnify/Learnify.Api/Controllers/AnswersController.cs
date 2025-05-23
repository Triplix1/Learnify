﻿using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class AnswersController : BaseApiController
{
    private readonly IAnswersService _answersService;

    public AnswersController(IAnswersService answersService)
    {
        _answersService = answersService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AnswersUpdatedResponse>> AddOrUpdateAnswerAsync(AnswerAddOrUpdateRequest answerAddOrUpdateRequest)
    {
        var userId = User.GetUserId();
        
        var response = await _answersService.AddOrUpdateAnswersAsync(userId, answerAddOrUpdateRequest);

        return Ok(response);
    }
}