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
    /// <returns></returns>
    public Task<ApiResponse> DeleteAsync(string id);
    
    /// <summary>
    /// Updates course's lesson and attachments
    /// </summary>
    /// <param name="id">Lesson id</param>
    /// <returns></returns>
    public Task<ApiResponse<LessonUpdateResponse>> GetForUpdateAsync(string id);

    /// <summary>
    /// Creates and updates async
    /// </summary>
    /// <param name="lessonAddOrUpdateRequest"></param>
    /// <returns></returns>
    public Task<ApiResponse<LessonUpdateResponse>> CreateOrUpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest);

    /// <summary>
    /// Gets lesson by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApiResponse<LessonResponse>> GetByIdAsync(string id);
}