using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class UserAuthorValidatorManager : IUserAuthorValidatorManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public UserAuthorValidatorManager(IPsqUnitOfWork psqUnitOfWork, IMongoUnitOfWork mongoUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task ValidateAuthorOfCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        var authorId = await _psqUnitOfWork.CourseRepository.GetAuthorIdAsync(courseId, cancellationToken);

        if (authorId is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        if (authorId != userId)
            throw new Exception("You have not permissions to update this course");
    }

    public async Task ValidateAuthorOfParagraphAsync(int paragraphId, int userId,
        CancellationToken cancellationToken = default)
    {
        var authorId = await _psqUnitOfWork.ParagraphRepository.GetAuthorIdAsync(paragraphId, cancellationToken);

        if (authorId is null)
            throw new KeyNotFoundException("Cannot find course with such Id");

        if (authorId != userId)
            throw new Exception("You have not permissions to update this course");
    }

    public async Task ValidateAuthorOfLessonAsync(string lessonId, int userId,
        CancellationToken cancellationToken = default)
    {
        var paragraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(lessonId, cancellationToken);

        await ValidateAuthorOfParagraphAsync(paragraphId, userId, cancellationToken: cancellationToken);
    }
}