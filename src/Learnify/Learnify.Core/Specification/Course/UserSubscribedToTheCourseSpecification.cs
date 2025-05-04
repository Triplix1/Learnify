using System.Linq.Expressions;
using Learnify.Core.Specification.Base;

namespace Learnify.Core.Specification.Course;

public class UserSubscribedToTheCourseSpecification: Specification<Domain.Entities.Sql.Course>
{
    public int UserId { get; set; }

    public UserSubscribedToTheCourseSpecification(int userId)
    {
        UserId = userId;
    }
    public override Expression<Func<Domain.Entities.Sql.Course, bool>> GetExpression()
    {
        return c => c.UserBoughts.Select(s => s.UserId).Contains(UserId);
    }
}