using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class UserValidatorManager: IUserValidatorManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;

    public UserValidatorManager(IPsqUnitOfWork psqUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
    }
    
    public async Task<Exception> ValidateAuthorOfCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        var authorId = await _psqUnitOfWork.CourseRepository.GetAuthorIdAsync(courseId, cancellationToken);

        if (authorId is null)
            return new KeyNotFoundException("Cannot find course with such Id");

        if (authorId != userId)
            return new Exception("You have not permissions to update this course");

        return null;
    }

    public async Task<Exception> ValidateAuthorOfParagraphAsync(int paragraphId, int userId,
        CancellationToken cancellationToken = default)
    {
        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(paragraphId, cancellationToken);

        if (authorId is null)
            return new KeyNotFoundException("Cannot find course with such Id");

        if (authorId != userId)
            return new Exception("You have not permissions to update this course");

        return null;
    }
}