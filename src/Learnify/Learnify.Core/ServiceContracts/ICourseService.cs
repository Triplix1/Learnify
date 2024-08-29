using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.ServiceContracts;

/// <summary>
/// Course service
/// </summary>
public interface ICourseService
{
    /// <summary>
    /// Returns filtered courses
    /// </summary>
    /// <returns>Filtered courses</returns>
    public Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync(CourseParams courseParams);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> GetByIdAsync(int id);

    public Task<ApiResponse<CourseResponse>> PublishAsync(int id, bool publish, int userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseCreateRequest"></param>
    /// <param name="userId">Author Id</param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseUpdateRequest"></param>
    /// <param name="userId">Author Id</param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId);

    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="userId">user Id</param>
    /// <returns></returns>
    public Task<ApiResponse> DeleteAsync(int id, int userId);
}