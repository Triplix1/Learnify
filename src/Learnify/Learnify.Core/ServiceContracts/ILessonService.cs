using Learnify.Core.Dto;

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
    public ApiResponse DeleteAsync(string id);
}