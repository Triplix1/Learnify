using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Params;

namespace Learnify.Core.ServiceContracts;

/// <summary>
/// Course service
/// </summary>
public interface ICourseService
{
    Task<PagedList<CourseTitleResponse>> GetAllCourseTitles(CancellationToken cancellationToken = default);

    Task<IEnumerable<CourseResponse>> GetFilteredAsync(CourseParams courseParams,
        CancellationToken cancellationToken = default);

    Task<CourseResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<CourseResponse> PublishAsync(int id, bool publish, int userId,
        CancellationToken cancellationToken = default);

    Task<CourseResponse> CreateAsync(CourseCreateRequest courseCreateRequest, int userId,
        CancellationToken cancellationToken = default);

    Task<CourseResponse> UpdateAsync(CourseUpdateRequest courseUpdateRequest, int userId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
}