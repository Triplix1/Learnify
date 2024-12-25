namespace Learnify.Core.ManagerContracts;

public interface IUserValidatorManager
{
    Task<Exception> ValidateAuthorOfCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);

    Task<Exception> ValidateAuthorOfParagraphAsync(int paragraphId, int userId,
        CancellationToken cancellationToken = default);
}