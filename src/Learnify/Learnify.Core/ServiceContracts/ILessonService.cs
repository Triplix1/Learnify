using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;

namespace Learnify.Core.ServiceContracts;

/// <summary>
/// Lesson service
/// </summary>
public interface ILessonService
{
    /// <summary>
    /// Deletes course's lesson and attachments
    /// </summary>
    /// <param name="id">Lesson id</param>
    /// <param name="userId">Current user id</param>
    /// <returns></returns>
    public Task<ApiResponse> DeleteAsync(string id, int userId);
    
    /// <summary>
    /// Updates course's lesson and attachments
    /// </summary>
    /// <param name="id">Lesson id</param>
    /// <param name="userId">Current user id</param>
    /// <returns></returns>
    public Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id, int userId);

    /// <summary>
    /// Creates and updates async
    /// </summary>
    /// <param name="lessonCreateRequest"><see cref="LessonCreateRequest"/></param>
    /// <param name="userId">Current user id</param>
    /// <returns></returns>
    public Task<ApiResponse<LessonResponse>> CreateAsync(LessonCreateRequest lessonCreateRequest, int userId);
    
    /// <summary>
    /// Creates and updates async
    /// </summary>
    /// <param name="lessonUpdateRequest"><see cref="LessonUpdateRequest"/></param>
    /// <param name="userId">Current user id</param>
    /// <returns></returns>
    public Task<ApiResponse<LessonResponse>> UpdateAsync(LessonUpdateRequest lessonUpdateRequest, int userId);

    /// <summary>
    /// Gets lesson by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId">Current user id</param>
    /// <returns></returns>
    public Task<ApiResponse<LessonResponse>> GetByIdAsync(string id, int userId);
}