using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;
using Learnify.Core.Specification;

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
    public Task<ApiResponse<CourseResponse>> GetById(string id);
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseCreateRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> CreateAsync(CourseCreateRequest courseCreateRequest, int authorId);
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="courseUpdateRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<CourseResponse>> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int authorId);
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApiResponse> DeleteAsync(string id);
}