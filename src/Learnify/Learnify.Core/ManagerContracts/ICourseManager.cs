namespace Learnify.Core.ManagerContracts;

public interface ICourseManager
{
    Task<Exception> ValidateAuthorOfCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);
}