namespace Learnify.Core.ManagerContracts;

public interface IUserBoughtValidatorManager
{
    Task ValidateUserAccessToTheCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default);
    Task ValidateUserAccessToTheLessonAsync(int userId, string lessonId, CancellationToken cancellationToken = default);
}