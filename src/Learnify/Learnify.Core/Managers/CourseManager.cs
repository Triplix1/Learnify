using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class CourseManager: ICourseManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;

    public CourseManager(IPsqUnitOfWork psqUnitOfWork)
    {
        _psqUnitOfWork = psqUnitOfWork;
    }

    public async Task<Exception> ValidateAuthorOfCourseAsync(int courseId, int userId)
    {
        var authorId = await _psqUnitOfWork.CourseRepository.GetAuthorId(courseId);

        if (authorId is null)
            return new KeyNotFoundException("Cannot find course with such Id");
            
        if(authorId != userId)
            return new Exception("You have not permissions to update this course");

        return null;
    }
}