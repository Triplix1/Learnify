using Microsoft.AspNetCore.Mvc;

namespace ProfileService.Controllers.Base;

/// <summary>
/// Base controller for all profile controllers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    
}