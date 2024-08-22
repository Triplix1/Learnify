using Learnify.Api.Controllers.Base;
using Learnify.Core.ServiceContracts;

namespace Learnify.Api.Controllers;

public class CourseController: BaseApiController
{
    private ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
}