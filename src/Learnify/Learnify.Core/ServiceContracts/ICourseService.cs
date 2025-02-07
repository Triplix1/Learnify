using Learnify.Core.Dto;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.ServiceContracts;

/// <summary>
/// Course service
/// </summary>
public interface ICourseService
{
    Task<PagedList<CourseTitleResponse>> GetAllCourseTitles(CourseParams courseParams, CancellationToken cancellationToken = default);
    Task<PagedList<CourseTitleResponse>> GetMyCourseTitles(int userId, CourseParams courseParams,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseResponse>> GetFilteredAsync(CourseParams courseParams,
        CancellationToken cancellationToken = default);
    Task<CourseMainInfoResponse> GetMainInfoResponseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);
    Task<CourseStudyResponse> GetCourseStudyResponseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);
    Task<CourseResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<CourseUpdateResponse> CreateAsync(CourseCreateRequest courseCreateRequest, int userId,
        CancellationToken cancellationToken = default);

    Task<PrivateFileDataResponse> UpdatePhotoAsync(int userId,
        PrivateFileBlobCreateRequest fileCreateRequest, CancellationToken cancellationToken = default);
    Task<PrivateFileDataResponse> UpdateVideoAsync(int userId,
        PrivateFileBlobCreateRequest fileCreateRequest, CancellationToken cancellationToken = default);
    Task<CourseUpdateResponse> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId,
        CancellationToken cancellationToken = default);
    Task<CourseUpdateResponse> PublishAsync(int id, bool publish, int userId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<bool> DeletePhotoAsync(int courseId, int userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteVideoAsync(int courseId, int userId, CancellationToken cancellationToken = default);
}