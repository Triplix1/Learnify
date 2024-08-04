using Learnify.Core.Dto;

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
    public Task<ApiResponse<IEnumerable<CourseResponse>>> GetFilteredAsync();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> GetById(string id);
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseCreateRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest);
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseUpdateRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest);
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApiResponse> DeleteAsync(string id);
}