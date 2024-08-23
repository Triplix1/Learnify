using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class ParagraphController: BaseApiController
{
    private readonly IParagraphService _paragraphService;

    public ParagraphController(IParagraphService paragraphService)
    {
        _paragraphService = paragraphService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ApiResponse<ParagraphResponse>> CreateAsync(ParagraphCreateRequest paragraphCreateRequest)
    {
        var userId = User.GetUserId();
        var result = await _paragraphService.CreateAsync(paragraphCreateRequest, userId);

        return result;
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ApiResponse<ParagraphResponse>> UpdateAsync(ParagraphUpdateRequest paragraphUpdateRequest)
    {
        var userId = User.GetUserId();
        var result = await _paragraphService.UpdateAsync(paragraphUpdateRequest, userId);

        return result;
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var userId = User.GetUserId();
        var result = await _paragraphService.DeleteAsync(id, userId);

        return result;
    }

}