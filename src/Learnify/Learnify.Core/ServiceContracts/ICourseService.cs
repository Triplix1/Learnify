using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.ServiceContracts;

/// <summary>
/// Course service
/// </summary>
public interface ICourseService
{
    Task<ApiResponse<PagedList<CourseTitleResponse>>> GetAllCourseTitles(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns filtered courses
    /// </summary>
    /// <returns>Filtered courses</returns>
    Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync(CourseParams courseParams,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ApiResponse<CourseResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResponse<CourseResponse>> PublishAsync(int id, bool publish, int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseCreateRequest"></param>
    /// <param name="userId">Author Id</param>
    /// <returns></returns>
    Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// <param name="courseUpdateRequest"></param>
    /// <param name="userId">Author Id</param>
    /// <returns></returns>
    Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="userId">user Id</param>
    /// <returns></returns>
    Task<ApiResponse> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
}