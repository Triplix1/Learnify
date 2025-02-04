using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Specification;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Courses repository
/// </summary>
public interface ICourseRepository
{
    Task<PagedList<Course>> GetFilteredAsync(EfFilter<Course> filter, CancellationToken cancellationToken = default);
    Task<Course> GetByIdAsync(int key, IEnumerable<string> includes = null, CancellationToken cancellationToken = default);
    Task<CoursePaymentResponse> GetCoursePaymentDataAsync(int id, CancellationToken cancellationToken = default);
    Task<int?> GetAuthorIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<int?> GetPhotoIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<int?> GetVideoIdAsync(int courseId, CancellationToken cancellationToken = default);

    Task<Course> CreateAsync(Course entity, CancellationToken cancellationToken = default);
    
    Task<Course> UpdateAsync(Course entity, CancellationToken cancellationToken = default);
    Task<bool> UpdatePhotoAsync(int courseId, int? photoId, CancellationToken cancellationToken = default);
    Task<bool> UpdateVideoAsync(int courseId, int? photoId, CancellationToken cancellationToken = default);
    Task<Course> PublishAsync(int key, bool publish, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}