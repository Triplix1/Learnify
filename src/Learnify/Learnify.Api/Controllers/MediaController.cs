using Learnify.Api.Controllers.Base;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class MediaController: BaseApiController
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;

    public MediaController(IPsqUnitOfWork psqUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
    }

    [HttpGet("course/{fileId}")]
    public async Task<ActionResult<Stream>> GetStreamForCourseFile(int fileId)
    {
        
    }
}