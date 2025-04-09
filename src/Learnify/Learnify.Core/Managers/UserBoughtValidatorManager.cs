using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Enums;
using Learnify.Core.ManagerContracts;

namespace Learnify.Core.Managers;

public class UserBoughtValidatorManager: IUserBoughtValidatorManager
{
    private readonly IPsqUnitOfWork _unitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public UserBoughtValidatorManager(IPsqUnitOfWork unitOfWork, IMongoUnitOfWork mongoUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task ValidateUserAccessToTheCourseAsync(int userId, int courseId, CancellationToken cancellationToken = default)
    {
        var authorId = await _unitOfWork.CourseRepository.GetCourseAuthorIdAsync(courseId, cancellationToken);
        
        if(authorId == userId)
            return;
        
        if (!await _unitOfWork.UserBoughtRepository.UserBoughtExistsAsync(userId, courseId, cancellationToken))
        {
            var currentUserRole = await _unitOfWork.UserRepository.GetUserRoleByIdAsync(userId, cancellationToken);
            
            if(!RoleLists.ModeratorsRoles.Contains(currentUserRole))
                throw new UnauthorizedAccessException("You do not have access to this course.");
        }
    }

    public async Task ValidateUserAccessToTheLessonAsync(int userId, string lessonId, CancellationToken cancellationToken = default)
    {
        var paragraphId = await _mongoUnitOfWork.Lessons.GetParagraphIdForLessonAsync(lessonId, cancellationToken);
        
        var courseId = await _unitOfWork.ParagraphRepository.GetCourseIdAsync(paragraphId, cancellationToken);

        if (courseId == null)
        {
            throw new UnauthorizedAccessException("Cannot find lesson in this course");
        }
        
        await ValidateUserAccessToTheCourseAsync(userId, courseId.Value, cancellationToken);
    }
}